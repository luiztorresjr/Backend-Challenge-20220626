

using MongoDB.Bson;
using WebScraping.Infra.Models;

namespace WebScraping.Model
{
    public class Product
    {
        public string? Id { get; set; }

        public long Code { get; set; }
        public string Barcode { get; set; } = string.Empty;

        public EStatus Status { get; set; }

        public DateTime Imported { get; set; }

        public string Url { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string Quantity { get; set; } = string.Empty;

        public string Categories { get; set; } = string.Empty;

        public string Packaging { get; set; } = string.Empty;

        public string Brands { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}