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
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        HttpClient client;

        public MovieController()
        {
            client = new HttpClient();
        }

        [HttpGet]
        public async Task<ActionResult<List<ExpandoObject>>> GetAllMovies()
        {
            try
            {
                var url = "https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo";
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<List<ExpandoObject>>();
                    return obj;
                }
                else
                    return Conflict("No entries in database");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpandoObject>> GetMovie(int id)
        {
            try
            {
                //var url = string.Format("https://localhost:44320/api/movieinfo/{0}", id.ToString());
                var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/{0}", id);
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<ExpandoObject>();
                    return obj;
                }
                else
                    return Conflict("No entry with id " + id);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("search/{title}")]
        public async Task<ActionResult<List<ExpandoObject>>> FindMovie(string title)
        {
            try
            {
                var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/title/{0}", title);
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<List<ExpandoObject>>();
                    return obj;
                }
                else
                    return NotFound("No movie with " + title + " was found");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
