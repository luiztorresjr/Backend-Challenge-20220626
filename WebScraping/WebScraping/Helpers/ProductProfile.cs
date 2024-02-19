using AutoMapper;
using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Helpers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductScraping, Product>().ReverseMap();
            CreateMap<ProductEntity, Product>().ReverseMap();
            CreateMap<ProductScraping, ProductEntity>().ReverseMap();

            CreateMap<Result<ProductEntity>, Result<Product>>().ReverseMap();
            CreateMap<Result<ProductScraping>, Result<ProductEntity>>().ReverseMap();
            CreateMap<Result<ProductScraping>, Result<Product>>().ReverseMap();

            CreateMap<EStatus, EStatusEntity>().ReverseMap();
        }
    }
}
