using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using P.Host.Models.Responses;
using Post.Host.Data.Entities;
using Post.Host.Models.Requests.Bases;
using Post.Host.Models.Responses;
using Post.Host.Services.Interfaces;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class Post—ommentController : ControllerBase
    {
        private readonly ILogger<Post—ommentController> _logger;
        private readonly IService<PostCommentEntity> _postCommentService;

        public Post—ommentController(ILogger<Post—ommentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BasePostCommentRequest request)
        {
            var result = await _postCommentService.AddAsync(new PostCommentEntity
            {
                Content = request.Content,
                PostId = request.PostId,
            });

            if (result == null)
                return BadRequest(new GeneralResponse(true, "Comment wasn't created"));

            return Ok(new GeneralResponse<int>(true, "Comment was created", result.Value));
        }

        [HttpPost]
        public Task<IActionResult> Update()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public Task<IActionResult> Delete()
        {

            return Ok();
        }
    }
}
