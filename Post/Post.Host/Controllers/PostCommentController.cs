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

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostCommentController : ControllerBase
    {
        private readonly ILogger<PostCommentController> _logger;
        private readonly IService<PostCommentEntity> _postCommentService;

        public PostCommentController(
            ILogger<PostCommentController> logger,
            IService<PostCommentEntity> postItemService)
        {
            _logger = logger;
            _postCommentService = postItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BasePostCommentRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCommentService.AddAsync(new PostCommentEntity
            {
                Content = request.Content,
                PostId = request.PostId,
            });

            if (response == null)
                return BadRequest(new GeneralResponse(false, "Comment wasn't created"));

            return Ok(new GeneralResponse<int>(true, "Comment was created", response.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostCommentRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCommentService.UpdateAsync(new PostCommentEntity
            {
                Id = request.Id,
                Content = request.Content,
                PostId = request.PostId,
            });

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Comment wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse<int>(true, "Comment was updated", response.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCommentService.DeleteAsync(request.Id);

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse(true, $"Post was deleted"));
        }
    }
}
