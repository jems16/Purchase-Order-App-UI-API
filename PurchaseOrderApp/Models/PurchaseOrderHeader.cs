namespace PurchaseOrderApp.Models
{
    public class PurchaseOrderHeader
    {
        public int Id { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string SalesName { get; set; } = string.Empty;
    }
}
