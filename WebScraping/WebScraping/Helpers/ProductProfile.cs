using AutoMapper;
using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<ProductScraping, Product>().ReverseMap();

            CreateMap<Result<ProductModel>, Result<Product>>().ReverseMap();
            CreateMap<Result<ProductScraping>, Result<Product>>().ReverseMap();
        }
    }
}
