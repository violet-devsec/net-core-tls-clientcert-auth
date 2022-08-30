using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ClientCertAuth.WebApi.Helpers;

namespace ClientCertAuth.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private string certName = "X-ARR-ClientCert";

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            #region Public Certificate Validation
            StringValues cert;
            if (!Request.Headers.TryGetValue(certName, out cert))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Invalid certificate!");
            }   

            bool certificateCheck = CertificateValidator.Verify(cert);
            if (!certificateCheck)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, "Certificate check failed!");
            }
            #endregion

            var rng = new Random();
            return new OkObjectResult(new WeatherForecast
            {
                Date = DateTime.Now.AddDays(1),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }
}
