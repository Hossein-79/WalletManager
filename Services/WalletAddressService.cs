using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Data;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class WalletAddressService : IWalletAddressService
    {
        private readonly WalletManagerContext _context;

        public WalletAddressService(WalletManagerContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task Add(WalletAddress walletAddress)
        {
            await _context.AddAsync(walletAddress);
            await _context.SaveChangesAsync();
        }

        public async Task<WalletAddress> GetWalletAddress(int id) =>
            await _context.WalletAddresses.Where(u => u.WalletAddressId == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<WalletAddress>> GetUserWallets(int userId) =>
            await _context.WalletAddresses.Where(u => u.UserId == userId).ToListAsync();
    }
}
