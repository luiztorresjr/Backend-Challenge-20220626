using AutoMapper;
using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductMongo, Product>().ReverseMap();
            CreateMap<ProductScraping, Product>().ReverseMap();
        }
    }
}
