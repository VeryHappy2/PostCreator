using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Services.Interfaces;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Catalog.Host.Models.Requests;
using Post.Host.Models.Requests.Bases;
using Post.Host.Models.Dtos;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;

namespace Post.Host.Controllers
{
    [ApiController]
    [EnableRateLimiting("Fixed")]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostCommentController : ControllerBase
    {
        private readonly ILogger<PostCommentController> _logger;
        private readonly IService<PostCommentEntity, PostCommentDto> _postCommentService;

        public PostCommentController(
            ILogger<PostCommentController> logger,
            IService<PostCommentEntity, PostCommentDto> postItemService)
        {
            _logger = logger;
            _postCommentService = postItemService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(BasePostCommentRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _postCommentService.AddAsync(new PostCommentEntity
            {
                Content = request.Content,
                PostId = request.PostId,
                UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value
            });

            if (response == null)
            {
                _logger.LogError("Comment wasn't created");
                return BadRequest(new GeneralResponse(false, "Comment wasn't created"));
            }

            return Ok(new GeneralResponse<int>(true, "Comment was created", response.Value!));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostCommentRequest request)
        {
            if (request == null)
            {
                _logger.LogError("Request is empty");
                return BadRequest(new GeneralResponse(false, "Request is empty"));
            }

            var response = await _postCommentService.UpdateAsync(new PostCommentEntity
            {
                Id = request.Id,
                Content = request.Content,
                PostId = request.PostId,
            });

            if (response == null)
            {
                _logger.LogError($"Comment wasn't update, not found, id of item: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Comment wasn't update, not found, id of item: {request.Id}"));
            }

            return Ok(new GeneralResponse<int>(true, "Comment was updated", response.Value!));
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

            var response = await _postCommentService.DeleteAsync(request.Id);

            if (response == null)
            {
                _logger.LogError($"Comment wasn't update, not found, id of item: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Comment wasn't update, not found id: {request.Id}"));
            }

            return Ok(new GeneralResponse(true, $"Post was deleted"));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<PostCommentEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(ByIdRequest<int> request)
        {
            if (request.Id == null)
            {
                return BadRequest(new GeneralResponse(false, "Id is null"));
            }

            var result = await _postCommentService.GetByIdAsync(request.Id);

            if (result == null)
            {
                return NotFound(new GeneralResponse(false, "Not found a post"));
            }

            return Ok(new GeneralResponse<PostCommentDto>(true, "Success", result));
        }
    }
}
