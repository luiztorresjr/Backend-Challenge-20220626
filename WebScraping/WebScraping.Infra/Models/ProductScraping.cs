using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Models
{
    public class ProductScraping 
    {
        public int? Id { get; set; }
        public long Code { get; set; }
        public string Barcode { get; set; }
        public EStatus Status { get; set; } = EStatus.imported;
        public DateTime Imported { get; set; } = DateTime.Now;
        public string Url { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Categories { get; set; }
        public string Packaging { get; set; }
        public string Brands { get; set; }
        public string ImageUrl { get; set; }


        public ProductScraping()
        {

        }

        public ProductScraping(long code, string barcode, EStatus status, DateTime imported, string url, string productName, string quantity, string categories, string packaging, string brands, string imageUrl)
        {
            Code = code;
            Barcode = barcode;
            Status = status;
            Imported = imported;
            Url = url;
            ProductName = productName;
            Quantity = quantity;
            Categories = categories;
            Packaging = packaging;
            Brands = brands;
            ImageUrl = imageUrl;
        }
    }
}