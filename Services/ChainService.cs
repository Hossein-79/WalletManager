using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Data;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class ChainService : IChainService
    {
        private readonly WalletManagerContext _context;

        public ChainService(WalletManagerContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task Add(Chain chain)
        {
            await _context.AddAsync(chain);
            await _context.SaveChangesAsync();
        }

        public async Task<Chain> GetChain(int id) =>
            await _context.Chains.Where(u => u.ChainId == id).FirstOrDefaultAsync();

        public async Task<Chain> GetChainByCovalentId(string covalentId) =>
            await _context.Chains.Where(u => u.CovalentId == covalentId).FirstOrDefaultAsync();

        public async Task<IEnumerable<Chain>> GetAllChains(bool testnet = false)
        {
            if (testnet)
                return await _context.Chains.ToListAsync();
            return await _context.Chains.Where(u => u.IsTestnet == false).ToListAsync();
        }

    }
}
