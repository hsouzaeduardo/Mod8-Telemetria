using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace FnGravarBlob
{
    public static class FnGravarBlob
    {

           
        [FunctionName("FnGravarBlob")]
        public static async Task<IActionResult> Run(
              [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest httpRequest,
            ILogger log)
        {
            string blobContainer = Environment.GetEnvironmentVariable("blobcontainer");
            string blobConnection = Environment.GetEnvironmentVariable("StorageConnectionString");
            string instrumentationKey = Environment.GetEnvironmentVariable("InstrumentationKey");
            BlobContainerClient container = new BlobContainerClient(blobConnection, blobContainer);

            var telemetryConfiguration = new TelemetryConfiguration(instrumentationKey);
            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            await container.CreateIfNotExistsAsync();

            var requestBody = await new StreamReader(httpRequest.Body).ReadToEndAsync();
            var pedido = JsonConvert.DeserializeObject<Pedido>(requestBody);
            var blobName =  $"{pedido.IdPedido.ToString("n")}-{pedido.DataPedido.ToString("yyyyMMdd")}.json";
            var cloudBlockBlob = container.GetBlobClient(blobName);
            var arquivoPedido = BinaryData.FromString(requestBody);
            await cloudBlockBlob.UploadAsync(arquivoPedido, overwrite: true);

            //Gerar Evento
            var evt = new EventTelemetry("FnGravarBlob executada");
            evt.Context.User.Id = pedido.Item;
            telemetryClient.TrackEvent(evt);

            //Gerar metrica Existente
            telemetryClient.GetMetric("contentLength").TrackValue(arquivoPedido.ToStream().Length);

            //Gerar metrica Customizada
            var pedidoTotal = new MetricTelemetry();
            pedidoTotal.Name = "Total em Vendas";
            pedidoTotal.Sum = Double.Parse(pedido.TotalPedido.ToString());
            telemetryClient.TrackMetric(pedidoTotal);


            return new OkObjectResult(blobName);
        }
    }
}
