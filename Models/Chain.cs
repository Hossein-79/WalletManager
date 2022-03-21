using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WalletManager.Models
{
    public class Chain
    {
        public int ChainId { get; set; }

        public string Name { get; set; }

        public bool IsTestnet { get; set; }

        public string Lable { get; set; }

        public string LogoUrl { get; set; }

        public string CovalentId { get; set; }
    }
}
