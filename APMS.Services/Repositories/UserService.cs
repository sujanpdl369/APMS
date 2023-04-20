using APMS.Common.Models;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace APMS.Services.Repositories
{
    public class UserService : IUserService
    {
        private readonly APMSDbContext _APMSDbContext;
        public UserService(APMSDbContext APMSDbContext)
        {
            _APMSDbContext = APMSDbContext;
        }
        public async Task<string> Register(RegisterVM register)
        {
            try
            {
                var userExists = await _APMSDbContext.Registers.Where(x => x.Email.Equals(register.Email)).AnyAsync();
                if (!userExists)
                {
                    var passwordHasher = new PasswordHasher<Register>();

                    Register regi = new Register
                    {
                        UserName = register.UserName,
                        MobileNumber = register.MobileNumber,
                        Email = register.Email,
                        Location = register.Location,
                        CreatedDate = DateTime.Now,
                    };
                    regi.Password = passwordHasher.HashPassword(regi, register.Password);
                    await _APMSDbContext.AddAsync(regi);
                    await _APMSDbContext.SaveChangesAsync();
                    return "Registered Successfully!";
                }
                else
                {
                    return "Email already Exist";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
