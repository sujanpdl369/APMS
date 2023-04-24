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
    }
}
