using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Services.Interfaces;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Catalog.Host.Models.Requests;
using IdentityModel;
using System.Net;
using System.Security.Claims;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostItemController : ControllerBase
    {
        private readonly ILogger<PostItemController> _logger;
        private readonly IService<PostItemEntity> _baseService;
        private readonly IPostItemService _postItemService;

        public PostItemController(
            ILogger<PostItemController> logger,
            IService<PostItemEntity> baseService,
            IPostItemService postItemService)
        {
            _postItemService = postItemService;
            _logger = logger;
            _baseService = baseService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(BasePostItemRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            string? userId = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            string? userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var response = await _baseService.AddAsync(new PostItemEntity
            {
                UserName = userName!,
                Date = DateTime.Now.Date.ToUniversalTime(),
                Title = request.Title,
                Content = request.Content,
                UserId = userId!,
                CategoryId = request.CategoryId,
            });

            if (response == null)
            {
                _logger.LogError("Post wasn't created");
                return BadRequest(new GeneralResponse(false, "Post wasn't created"));
            }

            return Ok(new GeneralResponse<int>(true, "Post was created", response.Value!));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostItemRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            string? userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            var response = await _baseService.UpdateAsync(new PostItemEntity
            {
                UserName = userName,
                Date = DateTime.Now.Date,
                Title = request.Title,
                Content = request.Content,
                UserId = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value,
                Id = request.Id,
                CategoryId = request.CategoryId,
            });

            if (response == null)
            {
                _logger.LogError($"Post wasn't update, not found id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));
            }

            return Ok(new GeneralResponse<int>(true, "Post was updated", response.Value!));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            string? userId = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id)?.Value;

            if (userId == null)
            {
                _logger.LogError("User id is empty");
                return Unauthorized(new GeneralResponse(false, "You need to log in"));
            }

            var post = await _baseService.GetByIdAsync(request.Id);

            if (post == null)
            {
                _logger.LogError("Post not found");
                return NotFound(new GeneralResponse(false, "Post not found"));
            }

            if (post.UserId != userId)
            {
                _logger.LogError("User isn't the owner of the post");
                return Unauthorized(new GeneralResponse(false, "User isn't the owner of the post"));
            }

            var response = await _baseService.DeleteAsync(request.Id);

            if (response == null)
            {
                _logger.LogError($"Post wasn't update, not found id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));
            }

            return Ok(new GeneralResponse(true, $"Post was deleted, {response.ToLower()}"));
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteByUserId(ByIdRequest<string> id)
        {
            if (string.IsNullOrEmpty(id.Id))
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "User id is empty"));
            }

            var result = await _postItemService.DeleteByUserIdAsync(id.Id);

            if (!result.Flag)
            {
                _logger.LogError($"Not found any posts by id: {id.Id}");
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddView(ByIdRequest<int> request)
        {
            if (request.Id != 1)
            {
                return BadRequest(new GeneralResponse(false, "Request can contain only one view"));
            }

            var result = await _postItemService.AddViewAsync(request.Id);

            if (result == null)
            {
                return BadRequest(new GeneralResponse(false, "Not found a post"));
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(ByIdRequest<int> request)
        {
            if (request.Id == null)
            {
                return BadRequest(new GeneralResponse(false, "Id is null"));
            }

            var result = await _baseService.GetByIdAsync(request.Id);

            if (result == null)
            {
                return NotFound(new GeneralResponse(false, "Not found a post"));
            }

            return Ok(new GeneralResponse<PostItemEntity>(true, "Success", result));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLike(ByIdRequest<int> request)
        {
            if (request.Id != 1)
            {
                return BadRequest(new GeneralResponse(false, "Request can contain only one view"));
            }

            var result = await _postItemService.AddLikeAsync(request.Id);

            if (result == null)
            {
                return BadRequest(new GeneralResponse(false, "Not found a post"));
            }

            return Ok(result);
        }
    }
}
