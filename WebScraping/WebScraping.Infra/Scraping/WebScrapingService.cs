using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        const string baseUrl = "https://world.openfoodfacts.org/";

        public async Task<List<ProductScraping>> GetProductUsingScraping()
        {
            List<ProductScraping> products = new List<ProductScraping>();
            HtmlWeb web = new HtmlWeb();

            HtmlDocument document = web.Load(baseUrl);

            // Select the product nodes
            HtmlNodeCollection productNodes = document.DocumentNode.SelectNodes("//div[@class='product']");

            if (productNodes != null)
            {
                foreach (HtmlNode productNode in productNodes)
                {


                    string imageUrl = productNode.SelectSingleNode(".//img")?.GetAttributeValue("src", "");
                    string barcode = productNode.SelectSingleNode(".//span[@itemprop='gtin13']")?.InnerText.Trim();
                    string commonName = productNode.SelectSingleNode(".//span[@itemprop='name']")?.InnerText.Trim();
                    string quantity = productNode.SelectSingleNode(".//span[@itemprop='quantity']")?.InnerText.Trim();
                    string packaging = productNode.SelectSingleNode(".//span[@itemprop='packaging']")?.InnerText.Trim();
                    string brands = productNode.SelectSingleNode(".//span[@itemprop='brand']")?.InnerText.Trim();
                    string categories = productNode.SelectSingleNode(".//span[@itemprop='category']")?.InnerText.Trim();
                    string productUrl = productNode.SelectSingleNode(".//a[@itemprop='url']")?.GetAttributeValue("href", "");
                    // Get product information
                    string pattern = "[a-zA-Z]";
                    // Replace all letters with an empty string
                    string result = Regex.Replace(barcode, pattern, "");
                    int code = int.Parse(result);
                    ProductScraping product = 
                        new ProductScraping(
                        code, 
                        barcode,
                        EStatus.imported, 
                        DateTime.Now, 
                        productUrl, 
                        commonName,
                        quantity, 
                        categories, 
                        brands, 
                        imageUrl);


                    // Output the scraped data
                    Console.WriteLine("Image URL: " + imageUrl);
                    Console.WriteLine("Barcode: " + barcode);
                    Console.WriteLine("Common Name: " + commonName);
                    Console.WriteLine("Quantity: " + quantity);
                    Console.WriteLine("Packaging: " + packaging);
                    Console.WriteLine("Brands: " + brands);
                    Console.WriteLine("Categories: " + categories);
                    Console.WriteLine("Product URL: " + productUrl);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No products found.");
            }

            return products;
        }
    }
}
