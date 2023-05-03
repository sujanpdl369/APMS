using APMS.Common;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmusementParkTicketManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly APMSDbContext _APMSDbContext;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IUserService _userService;
        public UserController(APMSDbContext APMSDbContext, IUserService userService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _APMSDbContext = APMSDbContext;
            _userService = userService;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
        }
        [HttpPost(nameof(Register))]
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

        [HttpPost(nameof(Login))]
        public async Task<ApiCallResponse> Login([FromBody] LoginVM loginVM)
        {
            UserController userController = this;
            try
            {
                if (!userController.ModelState.IsValid)
                {
                    return new ApiCallResponse()
                    {
                        Success = false,
                        ErrorCode = 0,
                        ErrorMessage = "Invalid Model",
                        Data = null
                    };
                }

                var register = await _userService.Login(loginVM);

                if (register == null)
                {
                    return new ApiCallResponse()
                    {
                        Success = false,
                        ErrorCode = 0,
                        ErrorMessage = "Invalid login credentials",
                        Data = null
                    };
                }

                List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, register.RegisterId.ToString())
        };
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(userController._configuration["JWT:Secret"]));
                string issuer = userController._configuration["JWT:ValidIssuer"];
                string audience = userController._configuration["JWT:ValidAudience"];
                DateTime? expires = DateTime.UtcNow.AddDays(Convert.ToDouble(userController._configuration["JWT:ExpirationInDays"]));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: credentials);

                return new ApiCallResponse()
                {
                    Success = true,
                    Data = new LoginRequestData()
                    {
                        auth_token = new JwtSecurityTokenHandler().WriteToken(token)
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 0,
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        [HttpPost(nameof(AddressDetail)), Authorize]
        public async Task<ApiCallResponse> AddressDetail(AddressDetailVM addressDetailVM)
        {
            var data = await _userService.UpdateAddressDetails(addressDetailVM);
            if(data is null)
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 0,
                    Data = null
                };
            }
            else
            {
                return new ApiCallResponse()
                {
                    Data = data,
                    Success = true
                };
            }
        }
        [HttpPost(nameof(Gender)), Authorize]
        public async Task<ApiCallResponse> Gender(string gender)
        {
            var data = await _userService.Genders(gender);
            if(data is null)
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Invalid Gender"
                };  
            }
            else
            {
                return new ApiCallResponse()
                {
                    Success = true,
                    Data = data,
                };
            }
        }

        
        [HttpGet(nameof(GetUserProfile)), Authorize]
        public async Task<ApiCallResponse> GetUserProfile()
        {
            var data = await _userService.GetUserProfile();
            return new ApiCallResponse()
            {
                Success = true,
                Data = data
            };
        }
        [HttpPost(nameof(UploadProfilePicture)), Authorize]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            byte[] profilePicture;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                profilePicture = memoryStream.ToArray();
            }

            var userId = _HttpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _userService.UploadProfilePicture(file, profilePicture);

            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }


        [HttpGet(nameof(GetProfilePicture)), Authorize]
        public async Task<IActionResult> GetProfilePicture()
        {
            var profilePicture = await _userService.GetProfilePictureAsync();

            if (profilePicture == null)
            {
                return NotFound();
            }
            // or "image/png", depending on the image format
            return File(profilePicture, "image/jpeg"); 
        }

        [HttpPut(nameof(UpdateUserProfile)), Authorize]
        public async Task<ApiCallResponse> UpdateUserProfile([FromBody] RegisterVM register)
        {
            try
            {
                var result = await _userService.UpdateUserProfile(register);
                if (result)
                {
                    return new ApiCallResponse()
                    {
                        Success = true,
                        Data = register,
                        ErrorMessage = null
                    };
                }
                else
                {
                    return new ApiCallResponse()
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "User not found"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = ex.Message
                };
            }
        }


        [HttpPost(nameof(ChangePassword)), Authorize]
        public async Task<ApiCallResponse> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            var response = await _userService.ChangePasswordAsync(changePasswordVM);
            if (response.StatusCode == 0)
            {
                return new ApiCallResponse()
                {
                    Success = true,
                    Data = response

                };
            }
            else
            {
                return new ApiCallResponse()
                {
                    Success = false,
                    ErrorCode = 0,
                    ErrorMessage = "Failed to Change Password",
                    Data = null
                };
            }
        }


    }
}
