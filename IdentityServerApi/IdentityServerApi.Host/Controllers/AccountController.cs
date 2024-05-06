using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityServerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class AccountController(IUserAccountRepository userAccount, SignInManager<UserEnity> signInManager) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            var response = await userAccount.CreateUserAccount(userRequest);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await userAccount.LoginAccount(loginRequest);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return Ok(new GeneralResponse(true, "User signed out"));
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeRole(ChangeRoleRequest loginRequest)
        {
            var response = await userAccount.ChangeRoleAccount(loginRequest);
            if (!response.Flag)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
