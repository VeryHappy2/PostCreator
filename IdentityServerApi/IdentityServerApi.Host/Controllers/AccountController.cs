using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityServerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class AccountController(IUserAccountRepository userAccount) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            var response = await userAccount.CreateUserAccount(userRequest);
            if (!response.Flag)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await userAccount.LoginAccount(loginRequest);
            if (!response.Flag)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
