using System.Collections.Generic;
using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface ICoinPriceService
    {
        Task Add(CoinPrice coinPrice);
        Task<IEnumerable<CoinPrice>> GetAllCoins();
        Task<CoinPrice> GetCoinPrice(string symbol);
        Task Update(CoinPrice coinPrice);
    }
}