using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletManager.Models.ViewModel
{
    public class WalletPreviewViewModel
    {
        public IEnumerable<Balance> Balances { get; set; }

        public decimal Total { get; set; }

        public int CoinCount { get; set; }

        public DateTime? FirstActivity { get; set; }
    }
}
