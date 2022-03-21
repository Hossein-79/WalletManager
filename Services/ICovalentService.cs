using System.Collections.Generic;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface ICovalentService
    {
        Task<IEnumerable<Chain>> GetAllChain();
    }
}