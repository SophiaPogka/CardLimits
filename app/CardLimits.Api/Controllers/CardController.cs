using CardLimits.Core.Model;
using CardLimits.Core.Services;
using CardLimits.Core.Services.Options;
using CardLimits.Core.Services.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CardLimits.api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {

        private readonly ILogger<CardController> _logger;
        private readonly ICardService _cards;
        private readonly ILimitService _limits;

        public CardController(ILogger<CardController> logger, ICardService cards, ILimitService limits)
        {
            _logger = logger;
            _cards = cards;
            _limits = limits;
        }

        [HttpGet]
        public string Get()
        {
            return "welcome!";

        }


        [HttpGet("{cardNumber}/type/{type:int}/amount/{amount:decimal}")]
        public IActionResult Authorize(string cardNumber, int type, decimal amount)
        {
            AuthorizeCardOptions options = new AuthorizeCardOptions {
                CardNumber = cardNumber,
                TransactionType = type,
                TransactionAmount = amount
            };

            var card = _cards.Authorize(options);

            return Json(card);
        }


        [HttpPost]
        public IActionResult UpdateCardLimit(
            [FromBody] AuthorizeCardOptions options)
        {
            var cust = _limits.UpdateCardLimit(options);

            return Json(cust);
        }
    }
}
