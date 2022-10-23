using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Linq;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB;

namespace FnGenerateDataTransacion
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
             [CosmosDB(
                databaseName:  "%databaseName%",
                collectionName: "%collectionName%",
                ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Pedido pedido = JsonConvert.DeserializeObject<Pedido>(requestBody);

            pedido.Endereco = await GetAddress(pedido.Cep);
            
            await documentsOut.AddAsync(new
            {
                // create a random ID
                id = System.Guid.NewGuid().ToString(),
                IdPedido = pedido.IdPedido,
                pedido = pedido
            });

            return new OkObjectResult(new {Message = "Pedido Criado com Sucesso.", Data = pedido});
        }

        private static async Task<Endereco> GetAddress(string cep)
        {
            Endereco enderecoResult = null;
            using(HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");
                enderecoResult = JsonConvert.DeserializeObject<Endereco>(result);
            }

            return enderecoResult;
        }
    }
}
