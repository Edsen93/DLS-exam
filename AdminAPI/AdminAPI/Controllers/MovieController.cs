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

        [HttpPost]
        public async Task<ActionResult<HttpResponseMessage>> CreateMovie([FromBody]ExpandoObject movie)
        {
            try
            {
                var realMovie = new ExpandoObject();
                if (string.Equals(movie.ElementAt(0).Key.ToLower(), "id"))
                {
                    realMovie.TryAdd(movie.ElementAt(1).Key, movie.ElementAt(1).Value);
                    realMovie.TryAdd(movie.ElementAt(2).Key, movie.ElementAt(2).Value);

                }
                else
                    realMovie = movie;

                //var url = "https://localhost:44320/api/movieinfo/";
                var url = "https://dlsmoviemicroservice.azurewebsites.net/api/movieinfo/";
                var content = await client.PostAsJsonAsync<ExpandoObject>(url, realMovie);
                if (content.IsSuccessStatusCode)
                {
                    var msg = await content.Content.ReadAsAsync<ExpandoObject>();
                    var id = new ExpandoObject();
                    var result = id.TryAdd(msg.ElementAt(0).Key, msg.ElementAt(0).Value);
                    if (result)
                    {
                        id.TryAdd(msg.ElementAt(1).Key, msg.ElementAt(1).Value);
                        id.TryAdd(msg.ElementAt(2).Key, msg.ElementAt(2).Value);
                        id.TryAdd(movie.ElementAt(3).Key, movie.ElementAt(3).Value);
                       
                        //url = "https://localhost:44319/api/User";
                        url = "https://dlsrecommendmicroservice.azurewebsites.net/api/movie";
                        content = await client.PostAsJsonAsync<ExpandoObject>(url, id);
                        if (content.IsSuccessStatusCode)
                            return content;
                        else
                            return Conflict("Something went wrong adding id to Neo4J");
                    }
                    else
                        return Conflict("Could not find user id");
                }
                else
                    return Conflict("User exist or does not match the user object");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HttpResponseMessage>> UpdateMovie(int id, [FromBody]ExpandoObject movie)
        {
            try
            {
                var realMovie = new ExpandoObject();
                if (string.Equals(movie.ElementAt(0).Key.ToLower(), "id"))
                {
                    realMovie.TryAdd(movie.ElementAt(1).Key, movie.ElementAt(1).Value);
                    realMovie.TryAdd(movie.ElementAt(2).Key, movie.ElementAt(2).Value);

                }
                else
                    realMovie = movie;

                var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/users/{0}", id);
                var content = await client.PutAsJsonAsync<ExpandoObject>(url, realMovie);
                if (content.IsSuccessStatusCode)
                {
                    var msg = await content.Content.ReadAsAsync<ExpandoObject>();
                    var obj = new ExpandoObject();
                    var result = obj.TryAdd(msg.ElementAt(1).Key, msg.ElementAt(1).Value);
                    if (result)
                    {
                        obj.TryAdd(msg.ElementAt(2).Key, msg.ElementAt(2).Value);
                        obj.TryAdd(movie.ElementAt(3).Key, movie.ElementAt(3).Value);

                        //url = "https://localhost:44319/api/User";
                        url = "https://dlsrecommendmicroservice.azurewebsites.net/api/movie";
                        content = await client.PutAsJsonAsync<ExpandoObject>(url, obj);
                        if (content.IsSuccessStatusCode)
                            return content;
                        else
                            return Conflict("Something went wrong adding id to Neo4J");
                    }
                    else
                        return Conflict("Could not find user id");
                }
                else
                    return BadRequest("Id does not exist or is not an integer");
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponseMessage>> DeleteMovie(int id)
        {
            try
            {
                var url = string.Format("https://dlsmoviemicroservice.azurewebsites.net/api/users/{0}", id);
                var content = await client.DeleteAsync(url);
                if (content.IsSuccessStatusCode)
                {
                    url = string.Format("https://dlsrecommendmicroservice.azurewebsites.net/api/User/{0}", id);
                    content = await client.DeleteAsync(url);
                    if (content.IsSuccessStatusCode)
                        return content;
                    else
                        return Conflict("Something went wrong deleting from Neo4J");
                }
                else
                    return BadRequest("Id does not exist or is not an integer");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
