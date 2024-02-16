using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using WebScraping.Infra.Models;
using WebScraping.Infra.Services;


namespace WebScraping.Infra.Scraping
{
    public class WebScrapingService : IWebScrapingService
    {
        const string baseUrl = "https://world.openfoodfacts.org";
        private IProductMongoService _service;
        private readonly ILogger<WebScrapingService> _logger;

        public WebScrapingService(IProductMongoService service, ILogger<WebScrapingService> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<List<ProductEntity>> GetProductUsingScraping()
        {
            
            List<ProductEntity> products = new List<ProductEntity>();
            List<String> hrefTags = new List<String>();
            var web = new HtmlWeb();
            HtmlDocument document = web.Load(baseUrl);

            // Select the product nodes
            var pagination = document.DocumentNode.SelectNodes("//ul[@class='pagination']//a[@href]");
            Console.WriteLine(pagination.Count);
            int pages = int.Parse(pagination[pagination.Count - 3].InnerText);
            for (int i = 1; i <= pages; i++)
            {
                document = web.Load(baseUrl + ($"/{i}"));
                foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];
                    if (att.Value.Contains("/product/"))
                    {
                        var product = new ProductEntity(GetProduct((baseUrl + att.Value), att.Value));

                        if (!await _service.GetByCodeExistsAsync(product.Code))
                        {
                            hrefTags.Add(att.Value);
                            products.Add(product);
                            
                        }
                        if (hrefTags.Count == 100)
                        {
                            _logger.LogInformation("Ultimo passado " + product);
                            try
                            {
                                await _service.CreateMany(products);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                _logger.LogError(ex.ToString());
                            }
                            return products;
                        }
                    }

                }
            }
            try
            {
                await _service.CreateMany(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex.ToString());
            }
            _logger.LogInformation("Lista finalizada");
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
            ProductScraping product = new();
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