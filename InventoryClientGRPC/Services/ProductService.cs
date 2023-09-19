using Grpc.Core;
using Inventory.Services.Product;
using InventoryClientGRPC.Protos;

namespace InventoryClientGRPC.Services
{
    public class ProductService : Protos.ProductProtoService.ProductProtoServiceBase
    {
        IProductService service;
        public ProductService(IProductService service) { 
            this.service = service;
        }


        public override async Task<GetProductResponse> getProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await service.GetOne(int.Parse(request.ProductId));
            if(product.Name == null)
            {
                return new GetProductResponse();
            }
            return new GetProductResponse()
            {
                Price = ((double)product.Price),
                Name = product.Name,
            };
        }
    }
}
