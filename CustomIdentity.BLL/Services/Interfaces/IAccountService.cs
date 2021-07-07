using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Account;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResultModel> LoginAsync(LoginModel loginModel, bool lockoutOnFailure = false);
        Task RegisterAsync(RegisterModel registerModel);
    }
}
