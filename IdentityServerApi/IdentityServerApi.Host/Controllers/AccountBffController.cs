using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using IdentityServerApi.Models;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityServerApi.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class AccountBffController : ControllerBase
    {
        private readonly IUserBffAccountService _userBffAccountService;
        public AccountBffController(IUserBffAccountService userBffAccountService)
        {
            _userBffAccountService = userBffAccountService;
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse<List<SearchAdminUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchByNameAdmin(ByNameRequest<string> userName)
        {
            if (userName == null)
            {
                return BadRequest(new GeneralResponse(false, "The request is empty"));
            }

            var result = await _userBffAccountService.AdminGetUsersByNameAsync(userName.Name);

            if (!result.Flag)
            {
                return NotFound(new GeneralResponse(result.Flag, result.Message));
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<List<SearchUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchByNameUser(ByNameRequest<string> userName)
        {
            if (userName == null)
            {
                return BadRequest(new GeneralResponse(false, "The request is empty"));
            }

            var result = await _userBffAccountService.UserGetUsersByNameAsync(userName.Name);

            if (!result.Flag)
            {
                return NotFound(new GeneralResponse(result.Flag, result.Message));
            }

            return Ok(result);
        }
    }
}
