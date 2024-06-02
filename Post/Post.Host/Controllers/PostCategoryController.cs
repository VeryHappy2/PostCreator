using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Host.Services.Interfaces;
using Post.Host.Data.Entities;
using Post.Host.Models.Responses;
using Catalog.Host.Models.Requests;
using Post.Host.Models.Dtos;
using Post.Host.Models.Requests.Bases;
using System.Net;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    [Authorize(Roles = AuthRoles.User)]
    public class PostCategoryController : ControllerBase
    {
        private readonly ILogger<PostCategoryController> _logger;
        private readonly IService<PostCategoryEntity> _postCategoryService;

        public PostCategoryController(
            ILogger<PostCategoryController> logger,
            IService<PostCategoryEntity> postItemService)
        {
            _logger = logger;
            _postCategoryService = postItemService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Add(BasePostCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCategoryService.AddAsync(new PostCategoryEntity
            {
                Category = request.Category,
            });

            if (response == null)
                return BadRequest(new GeneralResponse(false, "Category wasn't created"));

            return Ok(new GeneralResponse<int>(true, "Category was created", response.Value!));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse<int>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdatePostCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCategoryService.UpdateAsync(new PostCategoryEntity
            {
                Category = request.Category,
                Id = request.Id
            });

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Category wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse<int>(true, "Category was updated", response.Value!));
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(GeneralResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(ByIdRequest<int> request)
        {
            if (request == null)
                return BadRequest(new GeneralResponse(false, "Request is empty"));

            var response = await _postCategoryService.DeleteAsync(request.Id);

            if (response == null)
                return NotFound(new GeneralResponse(false, $"Category wasn't update, not found id: {request.Id}"));

            return Ok(new GeneralResponse(true, $"Category was deleted, {response.ToLower()}"));
        }
    }
}
