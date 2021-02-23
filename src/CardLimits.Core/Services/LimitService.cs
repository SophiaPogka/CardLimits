using CardLimits.Core.Consts;
using CardLimits.Core.Data;
using CardLimits.Core.Model;
using CardLimits.Core.Services.Options;
using CardLimits.Core.Services.Results;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLimits.Core.Services
{
    public class LimitService : ILimitService
    {

        private CardDbContext _dbContext;
        private ICardService _card;

        public LimitService(CardDbContext dbContext, ICardService card)
        {
            _dbContext = dbContext;
            _card = card;

        }


        public Result<Limit> UpdateCardLimit(AuthorizeCardOptions options)
        {
            var result = _card.Authorize(options);
            if (result.Code != ResultCodes.Success)
                return new Result<Limit>()
                {
                    Code = result.Code,
                    Message = result.Message
                };


            //Κάνε το update στον πίνακα των καρτών το νέο balance
            result.Data.AvailableBalance -= options.TransactionAmount;

            //Κάνω commit τις αλλαγές στη βάση
            _dbContext.Update(result.Data);
            _dbContext.SaveChanges();

            //Τώρα θα πρέπει να κάνω και αλλαγή στον πίνακα των limits
            //Αν δεν υπάρχει εγγραφή, θα την προσθέσω, αλλιώς να κάνω update
            var limit = _dbContext.Set<Limit>()
                .Where(c => c.TransactionType == options.TransactionType)
                .Where(c => c.DateOfTransactions == DateTime.Today)
                .SingleOrDefault();

            //Εάν δε βρεθεί εγγραφή, τότε σημαίνει πως είναι η πρώτη συναλλαγή για σήμερα, οπότε προσθέτω νέα εγγραφή
            if (limit == null)
            {
                var newLimit = new Limit
                {
                    AggregateAmount = options.TransactionAmount,
                    TransactionType = options.TransactionType,
                    DateOfTransactions = DateTimeOffset.Now
                };

                var cardTemp = result.Data;
                cardTemp.Limits.Add(newLimit);
                //_dbContext.Add(newLimit);
                _dbContext.SaveChanges();
            }
            else 
            {
                limit.AggregateAmount += options.TransactionAmount;

                _dbContext.Update(limit);
                _dbContext.SaveChanges();
            
            }

            return new Result<Limit>()
            {
                Code = ResultCodes.Success,
                Message = $"The transaction has been commited."
            };

        }
    }
}
