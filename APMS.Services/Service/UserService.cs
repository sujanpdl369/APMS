using APMS.Common.Models;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Response = APMS.Common.ViewModel.Response;

namespace APMS.Services.Repositories
{
    public class UserService : IUserService
    {
        private readonly APMSDbContext _APMSDbContext;
        public UserService(APMSDbContext APMSDbContext)
        {
            _APMSDbContext = APMSDbContext;
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
                var response = _APMSDbContext.Registers.Where(c => c.UserName.Equals(loginVM.Username) && c.Password.Equals(loginVM.Password)).FirstOrDefault();
                if (!(response is null)) return response; else return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
