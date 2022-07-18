using Microsoft.AspNetCore.Mvc;

namespace mq_receiver_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqController : ControllerBase
    {
        public MqController()
        {
        }

        [HttpPost(Name = "StartReceiver")]
        public IActionResult Post()
        {
            return Ok(true);
        }
    }
}
