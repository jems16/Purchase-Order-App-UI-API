using Microsoft.AspNetCore.Mvc;
using PurchaseOrderApp.Models;
using System.Globalization;

namespace PurchaseOrderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private static List<PurchaseOrderHeader> _orders = new List<PurchaseOrderHeader>
        {
            new PurchaseOrderHeader
            {
                Id = 1,
                PONumber = "PO-001/Oct/25",
                SupplierName = "PT Niaga Bakti",
                OrderDate = DateTime.Now.AddDays(-2),
                PaymentMethod = "Cash",
                SalesName = "Jemrin"
            },
            new PurchaseOrderHeader
            {
                Id = 2,
                PONumber = "PO-002/Oct/25",
                SupplierName = "PT Sukses Mandiri",
                OrderDate = DateTime.Now.AddDays(-1),
                PaymentMethod = "Transfer",
                SalesName = "Jemrin"
            }
        };

        private string GeneratePONumber(DateTime orderDate)
        {
            var sameMonthCount = _orders
                .Count(p => p.OrderDate.Month == orderDate.Month && p.OrderDate.Year == orderDate.Year);

            int next = sameMonthCount + 1;
            var monthAbbr = orderDate.ToString("MMM", CultureInfo.InvariantCulture);
            var yy = orderDate.ToString("yy", CultureInfo.InvariantCulture);
            return $"PO-{next:D3}/{monthAbbr}/{yy}";
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orders.OrderByDescending(o => o.Id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] PurchaseOrderHeader input)
        {
            if (string.IsNullOrEmpty(input.SupplierName))
                return BadRequest("SupplierName is required");

            var newId = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
            input.Id = newId;
            input.OrderDate = input.OrderDate == default ? DateTime.Now : input.OrderDate;
            input.PONumber = GeneratePONumber(input.OrderDate);
            input.SalesName = "Jemrin";

            _orders.Add(input);
            return Ok(input);
        }
    }
}
