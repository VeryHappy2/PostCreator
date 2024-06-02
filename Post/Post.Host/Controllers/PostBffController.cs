using System.Net;
using Catalog.Host.Models.Requests;
using IdentityModel;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Data;
using Post.Host.Models.Dtos;
using Post.Host.Models.Response;
using Post.Host.Models.Responses;
using Post.Host.Services.Interfaces;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize]
    [Authorize(Roles = AuthRoles.User)]
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

        [HttpGet]
        public async Task<IActionResult> GetPostsByOwnUserId()
        {
            string? userId = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value;

            if (userId == null)
                return BadRequest(new GeneralResponse(false, "User id is empty"));

            var result = await _postBffService.GetPostsByUserIdAsync(userId);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"User id: {userId} wasn't found any posts"));

            return Ok(new GeneralResponse<List<PostItemDto>>(true, "Successfully", result));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse<PaginatedItemsResponse<PostItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostsByPage(PageItemRequest request)
        {
            if (request is null)
                return BadRequest(new GeneralResponse(false, "User id is empty"));

            var result = await _postBffService.GetPostByPageAsync(request);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"Not found any posts"));

            return Ok(new GeneralResponse<PaginatedItemsResponse<PostItemDto>>(true, "Successfully", result));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse<PaginatedItemsResponse<PostItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostsByUserId(ByIdRequest<string> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "User id is empty"));

            var result = await _postBffService.GetPostsByUserIdAsync(request.Id);

            if (result == null)
                return NotFound(new GeneralResponse(false, $"User id: {request} wasn't found any posts"));

            return Ok(new GeneralResponse<List<PostItemDto>>(true, "Successfully", result));
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse<List<PostCategoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostCategories()
        {
            var result = await _postBffService.GetPostCategoriesAsync();

            if (result == null)
                return NotFound(new GeneralResponse(false, $"Didn't find any categories"));

            return Ok(new GeneralResponse<List<PostCategoryDto>>(true, "Successfully", result));
        }
    }
}
