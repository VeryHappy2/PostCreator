using System.Net;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using IdentityServerApi.Models;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class AccountController : ControllerBase
    {
        private readonly IUserManagmentService _userManagmentService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(
            IUserManagmentService userManagmentService,
            ILogger<AccountController> logger,
            IUserRoleService userRoleService,
            IUserAuthenticationService userAuthenticationServic)
        {
            _userManagmentService = userManagmentService;
            _userRoleService = userRoleService;
            _userAuthenticationService = userAuthenticationServic;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh-token"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new GeneralResponse(false, "Refresh token is null"));
            }

            var response = await _userAuthenticationService.RefreshToken(refreshToken);

            if (response == null)
            {
                return BadRequest(new GeneralResponse(response.Flag, response.Message));
            }

            var tokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10),
            };

            Response.Cookies.Append("token", response.Data, tokenCookieOptions);

            return Ok(new GeneralResponse(response.Flag, response.Message));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            var response = await _userManagmentService.CreateUserAccountAsync(userRequest);
            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<UserLoginResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (Request.Cookies.ContainsKey("token"))
            {
                _logger.LogError("You're already logged in");
                return BadRequest(new GeneralResponse(false, "You're already logged in"));
            }

            var response = await _userAuthenticationService.LoginAccountAsync(loginRequest);

            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response));
                return BadRequest(response);
            }

            var tokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10),
            };

            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(10),
            };

            Response.Cookies.Append("token", response.AccessToken, tokenCookieOptions);
            Response.Cookies.Append("refresh-token", response.RefreshToken, refreshTokenCookieOptions);

            return Ok(new GeneralResponse<UserLoginResponse>(true, response.Message, new UserLoginResponse
            {
                Id = response.User.Id,
                UserName = response.User.UserName,
                Role = response.User.Role,
            }));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LogOut()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("refresh-token");

            return Ok(new GeneralResponse(true, "Exited"));
        }

        [HttpGet]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse<List<string>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _userRoleService.GetRolesAsync();

            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeRole(RoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _userRoleService.ChangeRoleAccountAsync(roleRequest);

            if (!response.Flag)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(ByNameRequest<string> userName)
        {
            if (string.IsNullOrEmpty(userName.Name))
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _userManagmentService.DeleteUserAccountAsync(userName.Name);

            if (!response.Flag)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
