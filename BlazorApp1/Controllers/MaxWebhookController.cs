using BlazorApp1.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Controllers
{
    [ApiController]
    [Route("api/max")]
    public class MaxWebhookController
        : ControllerBase
    {
        private readonly
            TaskBotService _bot;

        public MaxWebhookController(
            TaskBotService bot)
        {
            _bot = bot;
        }

        [HttpPost("message")]
        public async Task<IActionResult>
            ReceiveMessage(
                [FromBody]
                MessageRequest request)
        {
            var response =
                await _bot
                    .HandleMessage(
                        request.Text);

            return Ok(new
            {
                response
            });
        }
    }

    public class MessageRequest
    {
        public string Text
        {
            get;
            set;
        } = "";
    }
}