using CardLimits.Core.Model;
using CardLimits.Core.Services.Options;
using CardLimits.Core.Services.Results;

namespace CardLimits.Core.Services
{
    public interface ICardService
    {
        public Result<Card> Authorize(AuthorizeCardOptions options);

    }
}
