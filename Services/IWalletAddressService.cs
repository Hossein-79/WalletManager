using System.Collections.Generic;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface IWalletAddressService
    {
        Task Add(WalletAddress walletAddress);
        Task Delete(WalletAddress walletAddress);
        Task<IEnumerable<WalletAddress>> GetUserWallets(int userId);
        Task<WalletAddress> GetWalletAddress(int id);
    }
}