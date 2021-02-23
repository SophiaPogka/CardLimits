using CardLimits.Core.Consts;
using CardLimits.Core.Data;
using CardLimits.Core.Model;
using CardLimits.Core.Services.Options;
using CardLimits.Core.Services.Results;
using System;
using System.Linq;

namespace CardLimits.Core.Services
{
    public class CardService : ICardService
    {
        private CardDbContext _dbContext;

        public CardService(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Result<Card> Authorize(AuthorizeCardOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.CardNumber))
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"The CardNumber {options.CardNumber} is not valid"
                };
            }

            if (options.CardNumber.Length != 16)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"The CardNumber's lenght {options.CardNumber} is not valid. it must be 16 digits."
                };
            }

            if (options.TransactionAmount == 0M)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"The Transaction's amount {options.TransactionAmount} is zero"
                };
            }

            if (options.TransactionType != 1
                && options.TransactionType != 2)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"The Transaction Type {options.TransactionType} is not valid"
                };
            }


            var card = _dbContext.Set<Card>()
                .Where(c => c.CardNumber == options.CardNumber)
                .SingleOrDefault();


            if (card == null)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"The CardNumber {options.CardNumber} not found in the database."
                };

            }

            //Εάν έχει βρεθεί κάρτα στη βάση και είναι έγκυρη, ψαχνω στον πίνακα των Limit να δω εάν υπάρχει εγγραφή
            if (card.AvailableBalance >= options.TransactionAmount)
            {
                var limit = _dbContext.Set<Limit>()
                .Where(c => c.TransactionType == options.TransactionType)
                .Where(c => c.DateOfTransactions == DateTime.Today)
                .SingleOrDefault();

                //Εάν δε βρεθεί εγγραφή, τότε σημαίνει πως είναι η πρώτη συναλλαγή για σήμερα, οπότε είμαι οκ
                if (limit == null)
                {
                    return new Result<Card>()
                    {
                        Code = ResultCodes.Success,
                        Message = "The transaction can be proceed.",
                        Data = card
                    };
                }
                else 
                {
                    var transLimit = 0M;
                    if (options.TransactionType == 1)
                    {
                        transLimit = Consts.Consts.CardPresentLimit;
                    }
                    else 
                    {
                        transLimit = Consts.Consts.ECommerceLimit;
                    }

                    var localLimit = limit.AggregateAmount + options.TransactionAmount;
                    if (localLimit > transLimit)
                    {
                        return new Result<Card>()
                        {
                            Code = ResultCodes.NotAvailableBalance,
                            Message = "The transaction can not be proceed. Your transaction will exceed your daily limit."
                        };
                    }
                    else
                    {
                        return new Result<Card>()
                        {
                            Code = ResultCodes.Success,
                            Message = "The transaction can be proceed."
                        };
                    }
                }
            }
            else
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.Success,
                    Message = $"The CardNumber {options.CardNumber} has not available amount for this transaction.",
                    Data = card
                };


            }
        }

    }
}
