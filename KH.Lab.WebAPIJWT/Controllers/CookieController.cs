using Microsoft.AspNetCore.Mvc;
using Presco.Utility.Cookie;

namespace KH.Lab.WebAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookieController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<CookieController> _logger;
        private readonly ICookie _cookie;

        public CookieController(ILogger<CookieController> logger, ICookie cookie)
        {
            _logger = logger;
            _cookie = cookie;
        }

        [HttpGet(Name = "GetCookie")]
        public ActionResult<IEnumerable<WeatherForecast>> GetCookie()
        {
            var value = _cookie.Get<IEnumerable<WeatherForecast>>("WeatherForecast");
            return Ok(value ?? Enumerable.Empty<WeatherForecast>());
        }

        [HttpPost(Name = "SetCookie")]
        public IActionResult SetCookie()
        {
            var value = Enumerable.Range(1, 2).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            _cookie.Set("WeatherForecast", value);
            return Ok();
        }

        [HttpDelete(Name = "RemoveCookie")]
        public IActionResult RemoveCookie()
        {
            _cookie.Remove("WeatherForecast");
            return Ok();
        }
    }
}
