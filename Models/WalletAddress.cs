using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletManager.Models
{
    public class WalletAddress
    {
        public int WalletAddressId { get; set; }

        public int UserId { get; set; }

        public int ChainId { get; set; }

        public string Address { get; set; }

        public string Lable { get; set; }

        public User User { get; set; }

        public Chain Chain { get; set; }
    }
}
