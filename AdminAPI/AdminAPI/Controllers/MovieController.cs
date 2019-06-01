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
            var url = "https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo";
            string content = await client.GetStringAsync(url);
            List<ExpandoObject> movies;
            if (!string.IsNullOrEmpty(content))
                movies = JsonConvert.DeserializeObject<List<ExpandoObject>>(content);
            else
                movies = null;

            return movies;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpandoObject>> GetMovie(int id)
        {
            var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/{0}", id.ToString());
            string content = await client.GetStringAsync(url);

            ExpandoObject movie;
            if (!string.IsNullOrEmpty(content))
                movie = JsonConvert.DeserializeObject<ExpandoObject>(content);
            else
                movie = null;

            return movie;
        }

        [HttpPost]
        public async Task CreateMovie([FromBody]ExpandoObject movie)
        {
            var url = "https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/";
            var content = await client.PostAsJsonAsync<ExpandoObject>(url, movie);
            if (content.IsSuccessStatusCode)
            {
                    //url = "https://localhost:44319/api/Movie";
                    url = "https://dlsrecommendmicroservice.azurewebsites.net/api/Movie";
                    content = await client.PostAsJsonAsync<ExpandoObject>(url, movie);
            }
        }

        [HttpPut("{id}")]
        public async Task UpdateMovie(int id, [FromBody]ExpandoObject movie)
        {
            var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/{0}", id.ToString());
            var content = await client.PutAsJsonAsync<ExpandoObject>(url, movie);
        }

        [HttpGet("search/{title}")]
        public async Task<List<ExpandoObject>> FindMovie(string title)
        {
            var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/title/{0}", title);
            var content = await client.GetStringAsync(url);

            List<ExpandoObject> movies;
            if (!string.IsNullOrEmpty(content))
                movies = JsonConvert.DeserializeObject<List<ExpandoObject>>(content);
            else
                movies = null;

            return movies;
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/{0}", id.ToString());
            var content = await client.DeleteAsync(url);
        }
    }
}
