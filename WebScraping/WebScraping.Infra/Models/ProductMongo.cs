using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace WebScraping.Infra.Models
{
    public class ProductMongo
    {
        [BsonElement("_id")]
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; } = null;

        [BsonElement("code")]
        public double Code { get; set; }

        [BsonElement("barcode")]
        public string Barcode { get; set; } = String.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = String.Empty;

        [BsonElement("imported_t")]
        public DateTime Imported { get; set; }

        [BsonElement("url")]
        public string Url { get; set; } = String.Empty;
        [BsonElement("product_name")]
        public string ProductName { get; set; } = String.Empty;
        [BsonElement("quantity")]
        public string Quantity { get; set; } = String.Empty;
        [BsonElement("categories")]
        public string Categories { get; set; } = String.Empty;
        [BsonElement("packaging")]
        public string Packaging { get; set; } = String.Empty;
        [BsonElement("brands")]
        public string Brands { get; set; } = String.Empty;
        [BsonElement("image_url")]
        public string ImageUrl { get; set; } = String.Empty;
        public ProductMongo()
        {
                
        }

    }
}
