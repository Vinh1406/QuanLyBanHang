using Demo.Api.Filters;
using Demo.App.Controllers.Base;
using Demo.Domain;
using Demo.Domain.ApplicationServices.Users;
using Microsoft.AspNetCore.Mvc;

namespace Demo.App.Controllers.Public
{
    public class NoAuthController: NoAuthorizeController
    {
        private readonly IUserService _userService;

        public NoAuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<AuthorizedResponseModel> AuthorizedResponseModel(LoginViewModel login)
        {
            var result = await _userService.Login(login);
            return result;
        }
        [ApplicationAuthorize]
        [HttpPost]
        [Route("Register-Customer")]
        public async Task<ResponseResult> RegisterCustomer(RegisterUserViewModel model)
        {
            var result=await _userService.RegisterCustomer(model);
            return result;
        }
    }
}
