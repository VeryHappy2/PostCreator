using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Services.Interfaces;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Catalog.Host.Models.Requests;
using Post.Host.Models.Dtos;
using System.Net;
using Microsoft.AspNetCore.RateLimiting;

namespace Post.Host.Controllers
{
    [ApiController]
    [EnableRateLimiting("Fixed")]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostLikeController : ControllerBase
    {
        private readonly ILogger<PostCommentController> _logger;
        private readonly IService<PostLikeEntity, PostLikeDto> _postLikeService;

        public PostLikeController(
            ILogger<PostCommentController> logger,
            IService<PostLikeEntity, PostLikeDto> postItemService)
        {
            _logger = logger;
            _postLikeService = postItemService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(ByIdRequest<int> request)
        {
            if (request.Id == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _postLikeService.AddAsync(new PostLikeEntity
            {
                PostId = request.Id,
                UserId = User.Claims.FirstOrDefault(x => x.Type == "id").Value
            });

            if (response == null)
            {
                _logger.LogError("Like wasn't created");
                return BadRequest(new GeneralResponse(false, "Like wasn't created"));
            }

            return Ok(new GeneralResponse(true, "Like was created"));
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostLikeRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _postLikeService.UpdateAsync(new PostLikeEntity
            {
                PostId = request.PostId,
                UserId = request.UserId,
            });

            if (response == null)
            {
                _logger.LogError($"Like wasn't update, not found, id of item: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Like wasn't update, not found, id of item: {request.Id}"));
            }

            return Ok(new GeneralResponse<int>(true, "Like was updated", response.Value!));
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
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

            var response = await _postLikeService.DeleteAsync(request.Id);

            if (response == null)
            {
                _logger.LogError($"Like wasn't update, not found, id of item: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Like wasn't update, not found id: {request.Id}"));
            }

            return Ok(new GeneralResponse(true, $"Like was deleted"));
        }

        [HttpPost]
        [Authorize(Roles = AuthRoles.Admin)]
        [ProducesResponseType(typeof(GeneralResponse<PostLikeEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(ByIdRequest<int> request)
        {
            if (request.Id == null)
            {
                return BadRequest(new GeneralResponse(false, "Id is null"));
            }

            var result = await _postLikeService.GetByIdAsync(request.Id);

            if (result == null)
            {
                return NotFound(new GeneralResponse(false, "Not found a like"));
            }

            return Ok(new GeneralResponse<PostLikeDto>(true, "Success", result));
        }
    }
}
