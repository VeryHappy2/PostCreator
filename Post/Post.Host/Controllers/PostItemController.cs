using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Services.Interfaces;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Catalog.Host.Models.Requests;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostItemController : ControllerBase
    {
        private readonly ILogger<PostBffController> _logger;
        private readonly IService<PostItemEntity> _postItemService;

        public PostItemController(
            ILogger<PostBffController> logger,
            IService<PostItemEntity> postItemService)
        {
            _logger = logger;
            _postItemService = postItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BasePostItemRequest request)
        {
            var response = await _postItemService.AddAsync(new PostItemEntity
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                Title = request.Title,
                Content = request.Content,
                UserId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
                CategoryId = request.CategoryId,
            });
            if (response == null)
                return BadRequest(new GeneralResponse(false, "Post wasn't created"));

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostItemRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postItemService.UpdateAsync(new PostItemEntity
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                Title = request.Title,
                Content = request.Content,
                UserId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
                Id = request.Id,
                CategoryId = request.CategoryId,
            });

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse<int>(true, "Post was updated", response.Value!));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postItemService.DeleteAsync(request.Id);

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Post wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse(true, $"Post was deleted, {response.ToLower()}"));
        }
    }
}
