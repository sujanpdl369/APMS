using APMS.Common.LocalModel;
using APMS.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Utility = APMS.Common.LocalModel.Utility;

namespace APMS.API
{
    public class CustomPasswordHasher
    {
        public bool checkUserPassword(Register register, string password) => Utility.validatePassword(password, register.Password);

        public string HashPassword(Register register, string password) => throw new NotImplementedException();

        public PasswordVerificationResult VerifyHashedPassword(
          Register user,
          string hashedPassword,
          string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
