using CardLimits.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLimits.Core.Services.Options
{
    public class AuthorizeCardOptions
    {
        public string CardNumber { get; set; }
        public int TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }

    }
}
