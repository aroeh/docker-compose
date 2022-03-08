using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace weather_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConnectionMultiplexer cache;
        private readonly IDatabase cacheDb;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConnectionMultiplexer cacheCon)
        {
            _logger = logger;
            cache = cacheCon;
            cacheDb = cache.GetDatabase();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            _logger.LogInformation("checking cache for value");
            var cacheString = cacheDb.StringGet("weather");
            if (!string.IsNullOrWhiteSpace(cacheString))
            {
                _logger.LogInformation("data found in cache.  returning cached value");
                var cacheVal = JsonConvert.DeserializeObject<WeatherForecast[]>(cacheString);
                return Ok(cacheVal);
            }

            _logger.LogInformation("data not found in cache");
            _logger.LogInformation("retrieving data from repository");

            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.LogInformation("data found...adding to cache");
            cacheDb.StringSet("weather", JsonConvert.SerializeObject(weather));

            return Ok(weather);
        }
    }
}