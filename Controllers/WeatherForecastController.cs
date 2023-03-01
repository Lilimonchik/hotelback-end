using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    public WeatherForecastController()
    {
        
    }

    [HttpPost("GetWeatherForecast")]

    public IActionResult Method([FromBody] string Name)
    {
        return Ok(Name);
    }
}

