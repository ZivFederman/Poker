using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TexasHoldemLib;

namespace HoldemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldemHandController : ControllerBase
    {
        private readonly ILogger<HoldemHandController> logger;

        public HoldemHandController(ILogger<HoldemHandController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        public string EvaluateHand(List<Card> cards)
        {
            Hand result = HandEvaluator.EvalulateHandRank(cards);
            return JsonSerializer.Serialize(result);
        }
    }
}
