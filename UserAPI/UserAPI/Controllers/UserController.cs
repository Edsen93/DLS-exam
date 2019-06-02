using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        HttpClient client;

        public UserController()
        {
            client = new HttpClient();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpandoObject>> GetUser(int id)
        {
            try
            {
                //var url = string.Format("https://localhost:44320/api/users/{0}", id.ToString());
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
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

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<ExpandoObject>> Login(string username, string password)
        {
            try
            {
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}/{1}", username, password);
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<ExpandoObject>();
                    return obj;
                }
                else
                    return Conflict("Username or password is wrong");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<HttpResponseMessage>> CreateUser([FromBody]ExpandoObject user)
        {
            try
            {
                //var url = "https://localhost:44320/api/users/";
                var url = "https://dlsusermicroservice.azurewebsites.net/api/users/";

                if (user.Any(x => x.Value.ToString().ToLower() == "false" || x.Value.ToString().ToLower() == "f"))
                    return Forbid("User is not allowed to set admin as true");

                var content = await client.PostAsJsonAsync<ExpandoObject>(url, user);
                if (content.IsSuccessStatusCode)
                {
                    var msg = await content.Content.ReadAsAsync<ExpandoObject>();
                    if (msg.Any(x => x.Key.ToLower() == "userid"))
                    {
                        var id = new ExpandoObject();
                        id.TryAdd(msg.FirstOrDefault(x => x.Key.ToLower() == "userid").Key, msg.FirstOrDefault(x => x.Key.ToLower() == "userid").Value);
                        //url = "https://localhost:44319/api/User";
                        url = "https://dlsrecommendmicroservice.azurewebsites.net/api/User";
                        content = await client.PostAsJsonAsync<ExpandoObject>(url, id);
                        if (content.IsSuccessStatusCode)
                            return content;
                        else
                            return Conflict("Something went wrong adding id to Neo4J"); ;
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
        public async Task<ActionResult<HttpResponseMessage>> UpdateUser(int id, [FromBody]ExpandoObject user)
        {
            try
            {
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id);
                var content = await client.GetAsync(url);

                if (content.IsSuccessStatusCode)
                {
                    var obj = await content.Content.ReadAsAsync<ExpandoObject>();

                    var wantToBeAdmin = user.Any(x => x.Value.ToString().ToLower() == "true" || x.Value.ToString().ToLower() == "t");
                    var isNotAdmin = obj.Any(x => x.Value.ToString().ToLower() == "false" || x.Value.ToString().ToLower() == "f");

                    if (isNotAdmin && wantToBeAdmin)
                        return Forbid("User is not allowed to set admin as true");
                    else
                    {
                        content = await client.PutAsJsonAsync<ExpandoObject>(url, user);
                        if (content.IsSuccessStatusCode)
                            return content;
                        else
                            return BadRequest("Connection failed");
                    }
                }
                else
                    return Conflict("No entry with id " + id);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{uId}/{mId}/{rating}")]
        public async Task<ActionResult<HttpResponseMessage>> PostUserRating(long uId, long mId, int rating)
        {
            var url = string.Format("https://localhost:44319/api/User/ratemovie/{0}/{1}/{2}", uId, mId, rating);
            //var url = string.Format("https://dlsrecommendmicroservice.azurewebsites.net/api/User/ratemovie/{0}/{1}/{2}", uId, mId, rating);
            var content = await client.GetAsync(url);
            if (content.IsSuccessStatusCode)
                return content;
            else
                return Conflict("Rating has to be an integer");
        }

        //[HttpDelete("{id}")]
        //public async Task DeleteUser(int id)
        //{
        //    var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
        //    var content = await client.DeleteAsync(url);
        //}
    }
}
