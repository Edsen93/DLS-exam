using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminAPI.Controllers
{
    [Route("api/recommendation")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        HttpClient client;

        public RecommendationController()
        {
            client = new HttpClient();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ExpandoObject>>> GetRecommendation(int id)
        {
            var url = string.Format("https://dlsrecommendmicroservice.azurewebsites.net/api/Recommendation/{0}", id);
            string content = await client.GetStringAsync(url);
            List<ExpandoObject> recommends;
            if (!string.IsNullOrEmpty(content))
                recommends = JsonConvert.DeserializeObject<List<ExpandoObject>>(content);
            else
                recommends = null;

            return recommends;
        }
    }
}

