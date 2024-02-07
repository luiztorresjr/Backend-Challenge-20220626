using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using HtmlAgilityPack;
using WebScraping.Infra.Models;


namespace WebScraping.Infra.Scraping
{
    public class WebScrapingService : IWebScrapingService
    {
        const string baseUrl = "https://world.openfoodfacts.org";

        public async Task<List<ProductScraping>> GetProductUsingScraping()
        {
            List<ProductScraping> products = new List<ProductScraping>();
            List<String> hrefTags = new List<String>();
            var web = new HtmlWeb();
            HtmlDocument document = web.Load(baseUrl);

            // Select the product nodes
            var pagination  =  document.DocumentNode.SelectNodes("//ul[@class='pagination']//a[@href]");
            Console.WriteLine(pagination.Count);
            int pages = int.Parse(pagination[pagination.Count-3].InnerText);
            for( int i = 1; i <= pages; i++ ) {
                document = web.Load(baseUrl+($"/{i}"));
                foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];
                    if (att.Value.Contains("/product/"))
                    {
                        products.Add(GetProduct((baseUrl + att.Value),att.Value));
                    }
                }
            }

            
            return products;
        }
        public ProductScraping GetProduct(String url, string codigo)
        {
            string[] urlCode = codigo.Split("/");
            string cod = urlCode[2].Trim();
            long valorLong = 0L;

            if (long.TryParse(cod, out valorLong))
            {
                Console.WriteLine("Valor convertido para long: " + valorLong);
            }
            else
            {
                Console.WriteLine("A string não pode ser convertida para long: " + cod);
            }

            var web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            HtmlNode productInfoDiv = document.DocumentNode.SelectSingleNode("//div[@id='main_column']");
            ProductScraping product = new ProductScraping();
            if (productInfoDiv != null)
            {
                // Extracting product information
                string productName = productInfoDiv.SelectSingleNode(".//h1")?.InnerText.Trim() ?? "Sem nome";
                string productImageUrl = productInfoDiv.SelectSingleNode(".//img[@id='og_image']")?.GetAttributeValue("src", "") ?? "Sem informacao de Image";
                string barcode = productInfoDiv.SelectSingleNode(".//p[@id='barcode_paragraph']")?.InnerText.Replace("Barcode:", "").Trim() ?? "Sem informacao de codigo de barra";
                string quantity = productInfoDiv.SelectSingleNode(".//span[@id='field_quantity_value']")?.InnerText.Trim() ?? "Sem informacao quantidade";
                string packaging = productInfoDiv.SelectSingleNode(".//span[@id='field_packaging_value']")?.InnerText.Trim() ?? "Sem informcao de embalagem";
                string brands = productInfoDiv.SelectSingleNode(".//span[@id='field_brands_value']")?.InnerText.Trim() ?? "Sem infromacao de marca";
                string categories = productInfoDiv.SelectSingleNode(".//span[@id='field_categories_value']")?.InnerText.Trim() ?? "Sem informacao de categoria";
                string productUrl = url.Trim();
                long code = valorLong;
                product = new ProductScraping(code, barcode, EStatus.imported, DateTime.Now, productUrl, productName, quantity, categories, packaging, brands, productImageUrl);
            }
            else
            {
                Console.WriteLine("Product information not found.");
            }
            return product;
        }
    }

}
