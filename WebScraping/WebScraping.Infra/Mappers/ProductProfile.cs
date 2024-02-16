using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Mappers
{
    public class ProductEntityMapping : Profile
    {
        public ProductEntityMapping()
        {
            CreateMap<ProductEntity, ProductModel>().ReverseMap();
            CreateMap<ProductScraping, ProductEntity>().ReverseMap();

            CreateMap<Result<ProductEntity>, Result<ProductModel>>().ReverseMap();
            CreateMap<Result<ProductScraping>, Result<ProductEntity>>().ReverseMap();
        }
    }
}
