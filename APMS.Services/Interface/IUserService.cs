using APMS.Common.Models;
using APMS.Common.ViewModel;

namespace APMS.Services.Interface
{
    public interface IUserService
    {
        Task<Response> UserSignUp(RegisterVM register);
        Task<Register> Login(LoginVM loginVM);
    }
}
