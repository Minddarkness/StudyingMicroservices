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

namespace StudyingMicroservices.ProjectA.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet("hello")]
        public async Task<IActionResult> GetAsync()
        {
            var name = await RunGetNameAsync();

            string summaryLine = _hello;

            if (!string.IsNullOrEmpty(name))
            {
                summaryLine = summaryLine + ", " + name;
            }

            return Ok(summaryLine);
        }

        private static async Task<string> RunGetNameAsync()
        {
            // Update port # in the following line.

            if(_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:43393/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            string name = null;
            try
            {
                // Get name
                var json = await GetAsync("Name/randomName");
                name = JsonSerializer.Deserialize<string>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return name;
        }

        private static async Task<string> GetAsync(string uri)
        {
            using HttpResponseMessage response = await _client.GetAsync(uri);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await response.Content.ReadAsStringAsync();
        }

        private const string _hello = "Hello";
        static HttpClient _client = new HttpClient();
    }
}
