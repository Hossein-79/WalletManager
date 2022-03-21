using System.Threading.Tasks;
using WalletManager.Models;

namespace WalletManager.Services
{
    public interface IUserService
    {
        Task Add(User user);
        Task<User> GetUser(int useId);
        Task<User> GetUser(string name);
    }
}