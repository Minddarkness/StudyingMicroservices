using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;

namespace StudyingMicroservices.ProjectA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet("hello")]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var name = await RunGetNameAsync(cancellationToken);

            string summaryLine = _hello;

            if (!string.IsNullOrEmpty(name))
            {
                summaryLine = summaryLine + ", " + name;
            }

            return Ok(summaryLine);
        }

        private static async Task<string> RunGetNameAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Операция прервана");
                return string.Empty;
            }

            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:43393/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            string name = null;
            try
            {

                var json = await GetAsync("Name/randomName", cancellationToken);
                name = JsonSerializer.Deserialize<string>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return name;
        }

        private static async Task<string> GetAsync(string uri, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Операция прервана");
                return string.Empty;
            }

            using HttpResponseMessage response = await _client.GetAsync(uri);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await response.Content.ReadAsStringAsync();
        }

        private const string _hello = "Hello";
        private static HttpClient _client = new HttpClient();
    }
}
