using Catalog.Host.Models.Requests;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Models.Responses;
using Post.Host.Services.Interfaces;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Authorize(Roles = AuthRoles.User)]
    [Authorize(Roles = AuthRoles.Admin)]
    public class PostBffController : ControllerBase
    {
        private readonly ILogger<PostBffController> _logger;
        private readonly IPostBffService _postBffService;

        public PostBffController(
            ILogger<PostBffController> logger,
            IPostBffService postBffService)
        {
            _logger = logger;
            _postBffService = postBffService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(ByIdRequest<int> request)
        {
            var result = await _postBffService.GetPostByIdAsync(request.Id);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"Id: {request.Id} wasn't found any post"));

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetPostsByOwnUserId()
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            if (userId == null)
                return BadRequest(new GeneralResponse(false, "User id is empty"));

            var result = await _postBffService.GetPostsByUserIdAsync(userId);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"User id: {userId} wasn't found any posts"));

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsByUserId(ByIdRequest<string> userId)
        {
            if (userId == null)
                return BadRequest(new GeneralResponse(false, "User id is empty"));

            var result = await _postBffService.GetPostsByUserIdAsync(userId.Id);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"User id: {userId} wasn't found any posts"));

            return Ok(result);
        }
    }
}
