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
            try
            {
                var url = string.Format("https://dlsrecommendmicroservice.azurewebsites.net/api/Recommendation/{0}", id);
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<List<ExpandoObject>>();
                    return obj;
                }
                else
                    return Conflict("No recommendations were found");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}

