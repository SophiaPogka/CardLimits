using System;
using System.Collections.Generic;
using System.Text;

namespace CardLimits.Core.Model
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public decimal AvailableBalance { get; set; }
        public List<Limit> Limits { get; set; }


        public Card()
        {
            Limits = new List<Limit>();
        }
    }
}
