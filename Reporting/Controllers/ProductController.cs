using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reporting.GRPCServices;

namespace Reporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductGrpcService productGrpc;

        public ProductController(ProductGrpcService productGrpc)
        {
            this.productGrpc = productGrpc;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await productGrpc.GetProducts());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok(await productGrpc.GetProduct(id.ToString()));
        }
    }
}
