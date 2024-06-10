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

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostItemController : ControllerBase
    {
        private readonly ILogger<PostItemController> _logger;
        private readonly IService<PostItemEntity> _postItemService;

        public PostItemController(
            ILogger<PostItemController> logger,
            IService<PostItemEntity> postItemService)
        {
            _logger = logger;
            _postItemService = postItemService;
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

            if (userId == null)
            {
                _logger.LogError("User id is empty");
                return BadRequest(new GeneralResponse(false, "User is empty"));
            }

            var response = await _postItemService.AddAsync(new PostItemEntity
            {
                Date = DateTime.Now.Date.ToUniversalTime(),
                Title = request.Title,
                Content = request.Content,
                UserId = userId,
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

            var response = await _postItemService.UpdateAsync(new PostItemEntity
            {
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

            var response = await _postItemService.DeleteAsync(request.Id);

            if (response == null)
            {
                _logger.LogError($"Post wasn't update, not found id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));
            }

            return Ok(new GeneralResponse(true, $"Post was deleted, {response.ToLower()}"));
        }
    }
}
