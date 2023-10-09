using Grpc.Core;
using Inventory.Entities;
using Inventory.Protos;
using Inventory.Services.Product;
using System.ComponentModel;
using System.Linq.Expressions;
using static Inventory.GRPCServices.Utils;

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
            var tableName = "product";
            var enumParsed = Utils.ToEnum<EntityEnum>(tableName, EntityEnum.Product);
            var operationEnum = OperationEnum.FETCH;
            var allDynamicTable = await Utils.dynamicTableSwitch(enumParsed, operationEnum, service);
            var getProductsResponse = new GetProductsResponse();



            if (operationEnum == OperationEnum.FETCH)
            {
                foreach (var product in allDynamicTable.Products)
                {
                    getProductsResponse.Products.Add(new GetProductResponse()
                    {
                        Name = product.Name,
                        Price = ((double)product.Price)
                    });
                }
            }
            return getProductsResponse;
        }
    }

    public static class Utils
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            T result;
            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }

      

        public static async Task<AllDynamicTable> dynamicTableSwitch(EntityEnum entityEnum, OperationEnum operationEnum, IProductService service)
        {
            var fieldList = new List<DynamicField>() { new DynamicField() { Name = "Name" }, new DynamicField() { Name = "Price" } };
            var adsdsa = new Dictionary<string, object>();
            adsdsa.Add("Name", "bottle");
            adsdsa.Add("Price", 2m);


            AllDynamicTable allDynamicTable = new AllDynamicTable();
            switch (entityEnum)
            {
                case EntityEnum.Product:
                    if (operationEnum == OperationEnum.FETCH)
                    {
                        allDynamicTable.Products = await service.LoadAllWithRelatedAsync<Product>(x => x.Id, x => x.Price == 0);
                    }
                    else
                    {
                        service.SaveTableRecord<Product>(fieldList, adsdsa);
                    }
                    break;
                case EntityEnum.Sale:
                    if (operationEnum == OperationEnum.FETCH)
                    {
                        //allDynamicTable.Sales = await service.LoadAllWithRelatedAsync<Sale>();
                    }
                    else
                    {
                        service.SaveTableRecord<Sale>(fieldList, adsdsa);
                    }
                    break;
                default:
                    break;
            }
            return allDynamicTable;
        }
    }

    public enum EntityEnum
    {
        [Description("Default")] Default = 0,
        [Description("Product")] Product = 1,
        [Description("Sale")] Sale = 2
    }
    public enum OperationEnum
    {
        [Description("CREATE")] CREATE = 1,
        [Description("FETCH")] FETCH = 2
    }

    public class AllDynamicTable
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }

}
