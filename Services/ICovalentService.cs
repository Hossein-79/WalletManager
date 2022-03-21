using System.Collections.Generic;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface ICovalentService
    {
        Task<IEnumerable<Balance>> GetAddressBalance(string address, string chainId);
        Task<IEnumerable<Chain>> GetAllChain();
    }
}