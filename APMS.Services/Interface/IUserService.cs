using APMS.Common.ViewModel;

namespace APMS.Services.Interface
{
    public interface IUserService
    {
        Task<string> Register(RegisterVM register);
    }
}
