using Catalog.Host.Models.Requests;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Models.Requests.Bases;
using Post.Host.Models.Responses;
using Post.Host.Services.Interfaces;
using System.Net;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Authorize(Roles = AuthRoles.User)]
    public class Post—ommentController : ControllerBase
    {
        private readonly ILogger<Post—ommentController> _logger;
        private readonly IService<PostCommentEntity> _postCommentService;

        public Post—ommentController(
            ILogger<Post—ommentController> logger,
            IService<PostCommentEntity> postCommentService)
        {
            _postCommentService = postCommentService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(BasePostCommentRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var result = await _postCommentService.AddAsync(new PostCommentEntity
            {
                Content = request.Content,
                PostId = request.PostId,
            });

            if (result == null)
                return BadRequest(new GeneralResponse(false, "Comment wasn't created"));

            return Ok(new GeneralResponse<int>(true, "Comment was created", result.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostCommentRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var result = await _postCommentService.UpdateAsync(new PostCommentEntity
            {
                Content = request.Content,
                PostId = request.PostId,
                Id = request.Id
            });

            if (result == null)
            {
                _logger.LogError($"Comment wasn't found, id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Comment wasn't updated, id: {request.Id}"));
            }

            return Ok(new GeneralResponse<int>(true, "Comment was updated", result.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var result = await _postCommentService.DeleteAsync(request.Id);

            if (result == null)
            {
                _logger.LogError($"Comment wasn't found, id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Comment wasn't deleted, id: {request.Id}"));
            }

            _logger.LogInformation(result);
            return Ok(new GeneralResponse<string>(true, "Comment was updated", result));
        }
    }
}
