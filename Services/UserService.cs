using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Data;
using WalletManager.Models;

namespace WalletManager.Services
{
    public class UserService : IUserService
    {
        private readonly WalletManagerContext _context;

        public UserService(WalletManagerContext walletManagerContext)
        {
            _context = walletManagerContext;
        }

        public async Task Add(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(string name) =>
            await _context.Users.Where(u => u.Name == name).FirstOrDefaultAsync();

        public async Task<User> GetUser(int useId) =>
            await _context.Users.Where(u => u.UserId == useId).FirstOrDefaultAsync();
    }
}
