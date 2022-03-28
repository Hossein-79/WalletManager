using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Data
{
    public class WalletManagerContext : DbContext
    {
        public WalletManagerContext(DbContextOptions<WalletManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chain> Chains { get; set; }
        public DbSet<WalletAddress> WalletAddresses { get; set; }
        public DbSet<CoinPrice> CoinPrices { get; set; }
    }
}
