using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Post.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class PostBffController : ControllerBase
    {
        private readonly ILogger<PostBffController> _logger;

        public PostBffController(ILogger<PostBffController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetPostById()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
