using System;
using System.Collections.Generic;
using System.Text;

namespace CardLimits.Core.Model
{
    public class Limit
    {
        public int LimitId { get; set; }
        public int TransactionType { get; set; }
        public  decimal AggregateAmount{ get; set; }
        public DateTimeOffset DateOfTransactions { get; set; }
    }
}
