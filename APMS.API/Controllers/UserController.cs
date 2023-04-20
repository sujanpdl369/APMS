using APMS.Common;
using APMS.Common.ViewModel;
using APMS.Data;
using APMS.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AmusementParkTicketManagementSystem.Controllers
{
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly APMSDbContext _APMSDbContext;
        private readonly IUserService _userService;
        public UserController(APMSDbContext APMSDbContext, IUserService userService)
        {
            _APMSDbContext = APMSDbContext;
            _userService = userService;
        }
        [Route("api/[controller]/Register")]
        [HttpPost]
        public async Task<ApiCallResponse> Register(RegisterVM register)
        {
           var data = _userService.Register(register);
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
    }
}
