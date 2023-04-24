using APMS.API;
using APMS.Common;
using APMS.Common.Models;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace AmusementParkTicketManagementSystem.Controllers
{
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly APMSDbContext _APMSDbContext;
        private readonly IUserService _userService;
        public UserController(APMSDbContext APMSDbContext, IUserService userService, IConfiguration configuration)
        {
            _APMSDbContext = APMSDbContext;
            _userService = userService;
            _configuration = configuration;
        }
        [Route("api/[controller]/Register")]
        [HttpPost]
        public async Task<ApiCallResponse> Register(RegisterVM register)
        {
           var data = _userService.UserSignUp(register);
            if (data is not null)
            {
                return new ApiCallResponse()
                {
                    Success = true,
                    Data = data.Result
                };
            }
            else
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 404, 
                    ErrorMessage = "Cannot Register",
                    Data = null
                };
            }
        }

        [Route("api/[controller]/Login")]
        [HttpPost]
        public async Task<ApiCallResponse> Login([FromBody] LoginVM loginVM)
        {
            UserController userController = this;
            try
            {
                if (!userController.ModelState.IsValid)
                {
                    return new ApiCallResponse()
                    { Success = false, ErrorCode = 0, ErrorMessage = "Invalid Model", Data = null };
                }
                Register register = _APMSDbContext.Registers.Where<Register>((Expression<Func<Register, bool>>)(c => c.UserName == loginVM.Username)).FirstOrDefault<Register>();
                if (register == null)
                    return new ApiCallResponse()
                    {
                        Success = false,
                        ErrorCode = 0,
                        ErrorMessage = "User does not Exist"
                    };
                if (new CustomPasswordHasher().checkUserPassword(register, loginVM.Password))
                {
                    List<Claim> claimList1 = new List<Claim>()
          {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", loginVM.Username)
          };
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userController._configuration["JWT:Secret"]));
                    string issuer = userController._configuration["JWT:ValidIssuer"];
                    string audience = userController._configuration["JWT:ValidAudience"];
                    DateTime? nullable = new DateTime?(DateTime.UtcNow.AddYears(1));
                    List<Claim> claimList2 = claimList1;
                    SigningCredentials signingCredentials1 = new SigningCredentials((SecurityKey)key, "HS256");
                    DateTime? notBefore = new DateTime?();
                    DateTime? expires = nullable;
                    SigningCredentials signingCredentials2 = signingCredentials1;
                    JwtSecurityToken token = new JwtSecurityToken(issuer, audience, (IEnumerable<Claim>)claimList2, notBefore, expires, signingCredentials2);
                    return new ApiCallResponse()
                    {
                        Success = true,
                        Data = (object)new LoginRequestData()
                        {
                            auth_token = new JwtSecurityTokenHandler().WriteToken((SecurityToken)token)
                        }
                    };
                }
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 0,
                    Data = (object)null,
                    ErrorMessage = "Password does not match"
                };
            }
            catch (Exception ex)
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 0,
                    ErrorMessage = ex.Message,
                    Data = (object)null
                };
            }
        }


    }
}
