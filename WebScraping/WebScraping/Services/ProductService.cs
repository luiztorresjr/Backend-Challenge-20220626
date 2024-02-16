using Amazon.Runtime.Internal;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using WebScraping.Infra.Models;
using WebScraping.Infra.Services;
using WebScraping.Model;

namespace WebScraping.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductMongoService _service;

        public ProductService(IMapper mapper, IProductMongoService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Product?> AddProduct(Product produto)
        {
            var request = _mapper.Map<ProductModel>(produto);
            try {
                await _service.Create(request);
                return produto;

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task DeleteProduct(string id)
        {
            await _service.Remove(id);
        }

        public  Task<Result<Product>> GetAllProducts(int page, int pageSize)
        {
            var response = _service.Get(page, pageSize);
            var responseApi = new Result<Product>();
            if (response != null)
                responseApi = _mapper.Map<Result<Product>>(response);
            return Task.FromResult(responseApi);
        }

        public async Task<Product> GetProductById(string id)
        {
            var response = await _service.GetById(id);
            var responseApi = new Product();
            if (response != null)
                responseApi = _mapper.Map<Product>(response);
            return responseApi;
        }

        public async Task<Product?> UpdateProduct(string id, Product produto)
        {
            var request = _mapper.Map<ProductModel>(produto);
            try
            {
                await _service.Update(id, request);
                return produto;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

    }
}
