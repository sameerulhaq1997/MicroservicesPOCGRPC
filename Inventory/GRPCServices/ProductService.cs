using Grpc.Core;
using Inventory.Entities;
using Inventory.Protos;
using Inventory.Services.Product;


namespace Inventory.GRPCServices
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        IProductService service;
        public ProductService(IProductService service)
        {
            this.service = service;
        }


        public override async Task<GetProductResponse> getProduct(GetProductRequest request, ServerCallContext context)
        {
            int.TryParse(request.ProductId, out int productId);
            var product = await service.GetOne(productId);
            if (product.Name == null)
            {
                return new GetProductResponse();
            }
            return new GetProductResponse()
            {
                Price = ((double)product.Price),
                Name = product.Name,
            };
        }

        public override async Task<GetProductsResponse> getProducts(GetProductsRequest request, ServerCallContext context)
        {
            var products = await service.Get();
            var getProductsResponse = new GetProductsResponse();
            foreach (var product in products) 
            {
                getProductsResponse.Products.Add(new GetProductResponse()
                {
                    Name = product.Name,
                    Price = ((double)product.Price)
                });
            }
            return getProductsResponse;
        }
    }
}
