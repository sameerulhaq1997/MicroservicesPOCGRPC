//using Reporting.Protos;

using Inventory.Protos;

namespace Reporting.GRPCServices
{
    public class ProductGrpcService
    {
        private readonly ProductProtoService.ProductProtoServiceClient _protoService;

        public ProductGrpcService(ProductProtoService.ProductProtoServiceClient protoService)
        {
            _protoService = protoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GetProductResponse> GetProduct(string id)
        {
            try
            {
                var productRequest = new GetProductRequest { ProductId = id };
                var res = await _protoService.getProductAsync(productRequest);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetProductsResponse> GetProducts()
        {
            try
            {
                var productsRequest = new GetProductsRequest();
                var res = await _protoService.getProductsAsync(productsRequest);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
