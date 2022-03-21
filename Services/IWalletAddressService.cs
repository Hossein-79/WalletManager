using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface IWalletAddressService
    {
        Task Add(WalletAddress walletAddress);
        Task<WalletAddress> GetWalletAddress(int id);
    }
}