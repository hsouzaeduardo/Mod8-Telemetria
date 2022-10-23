using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders.Physical;
using System.Diagnostics;
using System.Net.Http;
using UIPedidos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace UIPedidos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Privacy(Pedidos pedidos)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                await CriarPedidoBlob(pedidos, client);
                await CriarDadoPedido(pedidos, client);
            }

            return RedirectToAction("Index");
        }
        private async Task CriarDadoPedido(Pedidos pedidos, HttpClient client)
        {
            var apiPedidos = _config.GetSection("apiDadosPedidos").Value;

            HttpContent content = new StringContent(JsonConvert.SerializeObject(pedidos));

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(apiPedidos.ToString()),
                Content = content
            };

            HttpResponseMessage result = await client.SendAsync(request);
        }

        private async Task CriarPedidoBlob(Pedidos pedidos, HttpClient client)
        {
            var apiPedidos = _config.GetSection("apiPedidos").Value;

            HttpContent content = new StringContent(JsonConvert.SerializeObject(pedidos));

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(apiPedidos.ToString()),
                Content = content
            };

            HttpResponseMessage result = await client.SendAsync(request);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}