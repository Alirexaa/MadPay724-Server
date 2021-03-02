using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {




        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string[] vs = new string[] { "value1", "value2" }.ToArray();
            return vs;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
            

    }
}
