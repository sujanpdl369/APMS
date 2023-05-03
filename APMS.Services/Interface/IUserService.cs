using APMS.Common.Models;
using APMS.Common.ViewModel;
using Microsoft.AspNetCore.Http;

namespace APMS.Services.Interface
{
    public interface IUserService
    {
        Task<Response> UserSignUp(RegisterVM register);
        Task<Register> Login(LoginVM loginVM);
        Task<Response> ChangePasswordAsync(ChangePasswordVM changePasswordVM);
        Task<Register> GetUserProfile();
        Task<bool> UpdateUserProfile(RegisterVM registerVM);
        Task<bool> UploadProfilePicture(IFormFile file, byte[] profilePicture);
        Task<byte[]?> GetProfilePictureAsync();
        Task<Response> AddUserDetails(AddressDetailVM addressDetailVM);
    }
}
