using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;
using WebScraping.Infra.Bases;

namespace WebScraping.Infra.Models
{
    public class ProductEntity : BaseModel
    {


        [BsonElement("barcode")]
        public string Barcode { get; set; } = String.Empty;

        [BsonElement("status")]
        public EStatusEntity Status { get; set; } = EStatusEntity.imported;

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
        public ProductEntity()
        {

        }

        public ProductEntity(long code, string barcode, EStatus status, DateTime imported, string url, string productName, string quantity, string categories, string packaging, string brands, string imageUrl)
        {
            Code = code;
            Barcode = barcode;
            Status = ComparaEnum(status);
            Imported = imported;
            Url = url;
            ProductName = productName;
            Quantity = quantity;
            Categories = categories;
            Packaging = packaging;
            Brands = brands;
            ImageUrl = imageUrl;
        }

        public ProductEntity(ProductScraping product)
        {
            Code = product.Code;
            Barcode = product.Barcode;
            Status = ComparaEnum(product.Status);
            Imported = product.Imported;
            Url = product.Url;
            ProductName = product.ProductName;
            Quantity = product.Quantity;
            Categories = product.Categories;
            Packaging = product.Packaging;
            Brands = product.Brands;
            ImageUrl = product.ImageUrl;
        }
        public EStatusEntity ComparaEnum(EStatus es)
        {
            return es switch
            {
                EStatus.draft => EStatusEntity.draft,
                EStatus.imported => EStatusEntity.imported,
                _ => EStatusEntity.imported,
            };
        }
    }


    public enum EStatusEntity
    {
        [Description("draft")]
        draft,
        [Description("imported")]
        imported
    }    
}
