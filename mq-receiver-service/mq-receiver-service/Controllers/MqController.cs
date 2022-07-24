using Microsoft.AspNetCore.Mvc;
using mq_receiver_service.DataAccess;

namespace mq_receiver_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MqController : ControllerBase
    {
        private readonly IMongoWeather mongoWeather;

        public MqController(IMongoWeather injectWeather)
        {
            mongoWeather = injectWeather;
        }

        [HttpGet(Name = "GetWeatherMessages")]
        public async Task<IActionResult> Get()
        {
            var messages = await mongoWeather.GetMessages();

            return Ok(messages);
        }
    }
}
