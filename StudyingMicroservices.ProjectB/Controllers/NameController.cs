using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyingMicroservices.ProjectB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NameController : Controller
    {
        [HttpGet("randomName")]
        public IActionResult Get()
        {
            var range = new Random();
            return Ok(_names[range.Next(0, _names.Length)]);
        }

        private string[] _names = {"Nastya", "Vlad", "Masha", "Lera"};
    }
}
