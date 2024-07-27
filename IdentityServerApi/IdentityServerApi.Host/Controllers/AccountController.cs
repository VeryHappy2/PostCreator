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
        private readonly IUserAccountService _userAccountRepository;
        private readonly ILogger<AccountController> _logger;
        public AccountController(
            IUserAccountService userAccountRepository,
            ILogger<AccountController> logger)
        {
            _userAccountRepository = userAccountRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            var response = await _userAccountRepository.CreateUserAccountAsync(userRequest);
            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response.Message));
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse<UserLoginResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (Request.Cookies.ContainsKey("token"))
            {
                _logger.LogError("You're already logged in");
                return BadRequest(new GeneralResponse(false, "You're already logged in"));
            }

            var response = await _userAccountRepository.LoginAccountAsync(loginRequest);

            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response));
                return BadRequest(response);
            }

            var tokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(60),
            };

            Response.Cookies.Append("token", response.Token, tokenCookieOptions);

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

            return Ok(new GeneralResponse(true, "Exited"));
        }

        [HttpGet]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse<List<string>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRoles()
        {
            var response = await _userAccountRepository.GetRoles();

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

            var response = await _userAccountRepository.ChangeRoleAccountAsync(roleRequest);

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

            var response = await _userAccountRepository.DeleteUserAccountAsync(userName.Name);

            if (!response.Flag)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
