using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletManager.Models
{
    public class CoinPrice
    {
        public int CoinPriceId { get; set; }

        public decimal Price { get; set; }

        public string Symbol { get; set; }

        public string ContractAddress { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
