using APMS.API;
using APMS.Common.Enum;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Register = APMS.Common.Models.Register;
using Response = APMS.Common.ViewModel.Response;

namespace APMS.Services.Repositories
{
    public class UserService : IUserService
    {
        private readonly APMSDbContext _APMSDbContext;
        private readonly IHttpContextAccessor _HttpContextAccessor;
     
        public UserService(APMSDbContext APMSDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _APMSDbContext = APMSDbContext;
            _HttpContextAccessor = httpContextAccessor;
           
        }
        public async Task<Response> UserSignUp(RegisterVM register)
        {
            if (await this._APMSDbContext.Registers.AnyAsync<Register>(u => u.UserName == register.UserName))
                return new Response()
                {
                    ResponseData = null,
                    StatusCode = 1,
                    StatusMessage = "UserName already exists"
                };
            string password = BCrypt.Net.BCrypt.HashPassword(register.Password);
            Register user = new Register()
            {
                Title = register.Title,
                FirstName = register.FirstName,
                LastName = register.LastName,
                DOB = register.DOB,
                UserName = register.UserName,
                MobileNumber = register.MobileNumber,
                Email = register.Email,
                Password = password,
                Location = register.Location,
                CreatedDate = DateTime.Now,
            };
            await _APMSDbContext.Registers.AddAsync(user);
            await _APMSDbContext.SaveChangesAsync();


            return new Response()
            {
                ResponseData = user,
                StatusCode = 0,
                StatusMessage = "Signed Up Successfully"
            };
        }
        public async Task<Register> Login(LoginVM loginVM)
        {
            try
            {
                var register = await _APMSDbContext.Registers.FirstOrDefaultAsync(c => c.UserName.Equals(loginVM.Username));
                if (register == null)
                {
                    return null;
                }

                if (new CustomPasswordHasher().checkUserPassword(register, loginVM.Password))
                {
                    return register;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response>AddUserDetails(AddressDetailVM addressDetailVM)
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return null;
            var register = _APMSDbContext.Registers.Where(i => i.RegisterId.ToString() == userId).FirstOrDefault();
            if (register == null)
                return null;
            register.Country = addressDetailVM.Country;
            register.City = addressDetailVM.City;
            register.State = addressDetailVM.State;
            register.Status = addressDetailVM.Status;
            if(addressDetailVM.gender == "Male")
            {
                register.Gender = Gender.Male;
            }
            else if(addressDetailVM.gender == "Female")
            {
                register.Gender = Gender.Female;
            }
            else if(addressDetailVM.gender == "Others")
            {
                register.Gender = Gender.Others;
            }
            else
            {
                return null;
            }
            
            try
            {
                await _APMSDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new Response()
            {
                ResponseData = register,
                StatusCode = 0,
                StatusMessage = "Address detail added Successfully"
            };

        }
       
        //Get User Detail for User Profile
        public async Task<Register> GetUserProfile()
        {
            var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var data = await _APMSDbContext.Registers.SingleOrDefaultAsync(x => x.RegisterId.ToString() == userId);
            return data;
        }

        //Update User Detail for User Profile

        public async Task<bool> UpdateUserProfile(RegisterVM registerVM)
        {
            try
            {
                var userId = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var existingUser = await _APMSDbContext.Registers.SingleOrDefaultAsync(x => x.RegisterId.ToString() == userId);
                if (existingUser == null)
                    return false;
                string password = BCrypt.Net.BCrypt.HashPassword(existingUser.Password);
                existingUser.FirstName = registerVM.FirstName;
                existingUser.LastName = registerVM.LastName;
                existingUser.Email = registerVM.Email;
                existingUser.MobileNumber = registerVM.MobileNumber;
                existingUser.Location = registerVM.Location;
                existingUser.DOB = registerVM.DOB;
                existingUser.Password = password;
                existingUser.UserName = registerVM.UserName;
                existingUser.Title = registerVM.Title;
                existingUser.ModifiedDate = DateTime.Now;
                _APMSDbContext.Registers.Update(existingUser);
                await _APMSDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public async Task<bool> UploadProfilePicture(IFormFile file, byte[] profilePicture)
        {
            var Id = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var register = await _APMSDbContext.Set<Register>().FirstOrDefaultAsync(x => x.RegisterId.ToString() == Id);

            if (register == null)
            {
                return false;
            }

            register.ProfilePicture = profilePicture;
            _APMSDbContext.Entry(register).State = EntityState.Modified;

            try
            {
                await _APMSDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<byte[]?> GetProfilePictureAsync()
        {
            var Id = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var register = await _APMSDbContext.Set<Register>().FirstOrDefaultAsync(x => x.RegisterId.ToString() == Id);
            if (register == null || register.ProfilePicture == null)
                return null;

            return register.ProfilePicture;
        }

        public async Task<Response> ChangePasswordAsync(ChangePasswordVM changePasswordVM)
        {
            var user = await _APMSDbContext.Registers.FirstOrDefaultAsync(u => u.UserName == changePasswordVM.userName);
            if (user.UserName == null || user.UserName != changePasswordVM.userName)
            {
                return new Response
                {
                    ResponseData = null,
                    StatusCode = 1,
                    StatusMessage = "Invalid UserName"
                };
            }

            // Verify the old password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(changePasswordVM.oldPassword, user.Password))
            {
                return new Response
                {
                    ResponseData = null,
                    StatusCode = 1,
                    StatusMessage = "Password does not match with old password"
                };

            }

            // Hash the new password using BCrypt
            var newHash = BCrypt.Net.BCrypt.HashPassword(changePasswordVM.newPassword);

            // Update the user's password in the database
            user.Password = newHash;
            _APMSDbContext.Update(user);
            var affectedRows = await _APMSDbContext.SaveChangesAsync();

            if (affectedRows > 0)
            {
                return new Response
                {
                    ResponseData = affectedRows,
                    StatusCode = 0,
                    StatusMessage = "Password Changed Successfully"
                };
            }
            else
            {
                return new Response
                {
                    ResponseData = null,
                    StatusCode = 0,
                    StatusMessage = "Failed to change Password"
                };
            }
        }
    }
}
