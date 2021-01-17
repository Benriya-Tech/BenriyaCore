using Benriya.Share.Models.SystemUsers;
using Benriya.Share.ViewModels;
using System.Threading.Tasks;

namespace Benriya.Core.Services
{    
    public interface IUserServices
    {
        string Login(Users idata);
        string Register(Users idata);
        Task<UserInfoModel> Authenticate(string username, string password);
    }
}