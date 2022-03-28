using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Data;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class CoinPriceService : ICoinPriceService
    {
        private readonly WalletManagerContext _context;

        public CoinPriceService(WalletManagerContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task<CoinPrice> GetCoinPrice(string symbol) =>
            await _context.CoinPrices.Where(u => u.Symbol == symbol).FirstOrDefaultAsync();

        public async Task<IEnumerable<CoinPrice>> GetAllCoins() =>
            await _context.CoinPrices.ToListAsync();

        public async Task Add(CoinPrice coinPrice)
        {
            await _context.AddAsync(coinPrice);
            await _context.SaveChangesAsync();
        }

        public async Task Update(CoinPrice coinPrice)
        {
            _context.Update(coinPrice);
            await _context.SaveChangesAsync();
        }
    }
}
