using System.Net;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdentityServerApi.Host.Controllers
{
    [ApiController]
    [Authorize(Roles = AuthRoles.Admin)]
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
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await _userAccountRepository.LoginAccountAsync(loginRequest);

            if (!response.Flag)
            {
                _logger.LogError(JsonConvert.SerializeObject(response));
                return BadRequest(response);
            }

            // var cookieOptions = new CookieOptions
            // {
            //     HttpOnly = true,
            //     SameSite = SameSiteMode.Strict,
            //     Expires = DateTime.UtcNow.AddMinutes(60)
            // };

            // Response.Cookies.Append("jwt", response.Token, cookieOptions);

            return Ok(response);
        }

        [HttpGet]
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
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _userAccountRepository.DeleteUserAccountAsync(userName);

            if (!response.Flag)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
