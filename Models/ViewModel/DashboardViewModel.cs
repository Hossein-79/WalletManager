using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletManager.Models.ViewModel
{
    public class DashboardViewModel
    {
        public IEnumerable<WalletAddress> Addresses { get; set; }

        public IEnumerable<Chain> Chains { get; set; }
    }
}
