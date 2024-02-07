using Amazon.Runtime.Internal;
using AutoMapper;
using WebScraping.Infra.Models;
using WebScraping.Infra.Services;
using WebScraping.Model;

namespace WebScraping.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IMongoDBService _service;

        public ProductService(IMapper mapper, IMongoDBService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<Product?> AddProduct(Product produto)
        {
           var request = _mapper.Map<ProductMongo>(produto);
            try{
                await _service.AddOneProduct(request);
                return produto;
            
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task DeleteProduct(string id)
        {
            await _service.DeleteProduct(id);
        }

        public async Task<List<Product>> GetAllProducts()
        {
                var response = await _service.GetAllProducts();
                var responseApi = new List<Product>();
                if(response != null)
                    responseApi = _mapper.Map<List<Product>>(response);
                return responseApi;
        }

        public async Task<Product> GetProductById(string id)
        {
            var response = await _service.GetProductById(id);
            var responseApi = new Product();
            if(response != null)
                responseApi = _mapper.Map<Product>(response);
            return responseApi;
        }

        public async Task<Product?> UpdateProduct(string id, Product produto)
        {
            var request = _mapper.Map<ProductMongo>(produto);
            try
            {
                await _service.UpdateProduct(id, request);
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
