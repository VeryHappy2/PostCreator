using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using IdentityServerApi.Models;
using Infrastructure;
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
        [ProducesResponseType(typeof(GeneralResponse<List<UserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchByName(ByNameRequest<string> userName)
        {
            if (userName == null)
            {
                return BadRequest(new GeneralResponse(false, "The request is empty"));
            }

            var result = await _userBffAccountService.GetUsersByNameAsync(userName.Name);

            if (!result.Flag)
            {
                return NotFound(new GeneralResponse(result.Flag, result.Message));
            }

            return Ok(result);
        }
    }
}
