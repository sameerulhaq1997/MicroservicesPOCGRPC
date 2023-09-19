using Microsoft.AspNetCore.Mvc;
using Reporting.GRPCServices;

namespace Reporting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        ProductGrpcService productGrpc;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ProductGrpcService productGrpc)
        {
            _logger = logger;
            this.productGrpc = productGrpc;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public async Task<IEnumerable<WeatherForecast>> Get()
        //{
        //    var dsaads = await productGrpc.GetProduct("1");
        //    return new List<WeatherForecast>();
        //}
    }
}