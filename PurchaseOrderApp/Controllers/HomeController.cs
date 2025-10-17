using Microsoft.AspNetCore.Mvc;
using PurchaseOrderApp.Models;
using System.Net.Http.Json;

namespace PurchaseOrderApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}")
            };

            var orders = await client.GetFromJsonAsync<List<PurchaseOrderHeader>>("/api/sales");
            return View(orders ?? new List<PurchaseOrderHeader>());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PurchaseOrderHeader order)
        {
            var client = _clientFactory.CreateClient();

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}")
            };

            var response = await client.PostAsJsonAsync("/api/sales", order);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to add Purchase Order");
                return View(order);
            }

            return RedirectToAction("Index");
        }
    }
}
