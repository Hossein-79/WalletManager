using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WalletManager.Models
{
    public class Balance
    {
        public int BalanceId { get; set; }

        public int WalletAddressId { get; set; }

        public decimal Value { get; set; }

        public string ContractName { get; set; }

        public string Symbol { get; set; }

        public string ContractAddress { get; set; }

        public string Type { get; set; }

        public string LogoUrl { get; set; }

        [NotMapped]
        public CoinPrice CoinPrice { get; set; }
    }
}
