using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Scraping
{
    public interface IWebScrapingService
    {
        Task<List<ProductEntity>> GetProductUsingScraping();
    }
}
