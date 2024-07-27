using System.Net;
using Catalog.Host.Models.Requests;
using IdentityModel;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Models.Dtos;
using Post.Host.Models.Response;
using Post.Host.Models.Responses;
using Post.Host.Services.Interfaces;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
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
        [ProducesResponseType(typeof(GeneralResponse<PostItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostById(ByIdRequest<int> request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var result = await _postBffService.GetPostByIdAsync(request.Id);

            if (result == null)
            {
                _logger.LogError($"Id: {request.Id} wasn't found any post");
                return NotFound(new GeneralResponse(false, $"Id: {request.Id} wasn't found any post"));
            }

            return Ok(new GeneralResponse<PostItemDto>(true, "Successfully", result));
        }

        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponse<List<PostItemDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostsByOwnUserId()
        {
            string? userId = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value;

            if (userId == null)
            {
                _logger.LogError($"User id is empty");
                return BadRequest(new GeneralResponse(false, "User id is empty"));
            }

            var result = await _postBffService.GetPostsByUserIdAsync(userId);

            if (result == null)
            {
                _logger.LogError($"In {userId} wasn't found");
                return NotFound(new GeneralResponse(false, $"User id: {userId} wasn't found any posts"));
            }

            return Ok(new GeneralResponse<List<PostItemDto>>(true, "Successfully", result));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse<List<PostItemDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostsByUserId(ByIdRequest<string> userId)
        {
            if (userId == null)
            {
                _logger.LogError($"User id is empty");
                return BadRequest(new GeneralResponse(false, "User id is empty"));
            }

            var result = await _postBffService.GetPostsByUserIdAsync(userId.Id);

            if (result == null)
            {
                _logger.LogError($"In {userId} wasn't found");
                return NotFound(new GeneralResponse(false, $"User id: {userId} wasn't found any posts"));
            }

            return Ok(new GeneralResponse<List<PostItemDto>>(true, "Successfully", result));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse<PaginatedResponse<PostItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostsByPage(PageItemRequest request)
        {
            if (request is null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "User id is empty"));
            }

            var result = await _postBffService.GetPostByPageAsync(request);

            if (result == null)
            {
                _logger.LogError($"Not found any posts.\nPage size: {request.PageSize}.\nPage index: {request.PageIndex}\nSearch: {request.SearchByTitle}\nCategory filter: {request.CategoryFilter}");
                return NotFound(new GeneralResponse(false, $"Not found any posts"));
            }

            return Ok(new GeneralResponse<PaginatedResponse<PostItemDto>>(true, "Successfully", result));
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(GeneralResponse<List<PostCategoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostCategories()
        {
            var result = await _postBffService.GetPostCategoriesAsync();

            if (result == null)
            {
                _logger.LogError("Didn't find any categories in data base");
                return NotFound(new GeneralResponse(false, $"Didn't find any categories"));
            }

            return Ok(new GeneralResponse<List<PostCategoryDto>>(true, "Successfully", result));
        }
    }
}
