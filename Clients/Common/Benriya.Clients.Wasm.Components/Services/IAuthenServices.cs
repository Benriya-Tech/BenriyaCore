using Benriya.Share.ViewModels;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components.Services
{
    public interface IAuthenServices
    {
        string Message { get; set; }
        bool isLoggedIn { get; set; }

        Task<UserInfoModel> GetUser();
        Task<bool> checkLoggedIn();
        Task<bool> Login(UserLoginModel idata);
        Task<bool> Logout();
        Task<bool> ReFreshToken();
        Task<string> GetToken();
    }
}