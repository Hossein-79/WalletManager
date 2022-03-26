using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface IChainService
    {
        Task Add(Chain chain);
        Task<IEnumerable<Chain>> GetAllChains(bool testnet = false);
        Task<Chain> GetChain(int id);
        Task<Chain> GetChainByCovalentId(string covalentId);
    }
}