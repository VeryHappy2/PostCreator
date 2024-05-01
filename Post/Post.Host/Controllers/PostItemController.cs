using Catalog.Host.Models.Requests;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Data.Entities;
using Post.Host.Models.Dtos;
using Post.Host.Models.Requests.Bases;
using Post.Host.Models.Responses;
using Post.Host.Services;
using Post.Host.Services.Interfaces;
using System.Net;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostItemController : ControllerBase
    {
        private readonly ILogger<PostÑommentController> _logger;
        private readonly IService<PostItemEntity> _postItemService;

        public PostItemController(
            ILogger<PostÑommentController> logger,
            IService<PostItemEntity> postItemService)
        {
            _postItemService = postItemService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add(BasePostItemRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));


            var result = await _postItemService.AddAsync(new PostItemEntity
            {
                Content = request.Content,
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                CategoryId = request.CategoryId,
                Title = request.Title,
                UserId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
            });

            if (result == null)
                return BadRequest(new GeneralResponse(false, "Post wasn't created"));

            return Ok(new GeneralResponse<int>(true, "Post was created", result.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostItemRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var result = await _postItemService.UpdateAsync(new PostItemEntity
            {
                Date = request.Date,
                CategoryId = request.CategoryId,
                Content = request.Content,
                Title = request.Title,
                UserId= User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
            });

            if (result == null)
            {
                _logger.LogError($"Post wasn't found, id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Post wasn't updated, id: {request.Id}"));
            }

            return Ok(new GeneralResponse<int>(true, "Post was updated", result.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var result = await _postItemService.DeleteAsync(request.Id);

            if (result == null)
            {
                _logger.LogError($"Post wasn't found, id: {request.Id}");
                return NotFound(new GeneralResponse(false, $"Post wasn't deleted, id: {request.Id}"));
            }

            _logger.LogInformation(result);
            return Ok(new GeneralResponse<string>(true, "Post was updated", result));
        }
    }
}
