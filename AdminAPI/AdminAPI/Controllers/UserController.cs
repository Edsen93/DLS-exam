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
using Steeltoe.Common.Discovery;

namespace AdminAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        HttpClient client;
        HttpClient eurekaClient;

        public UserController(IDiscoveryClient discclient)
        {
            var _handler = new DiscoveryHttpClientHandler(discclient);
            client = new HttpClient();
            eurekaClient = new HttpClient(_handler, false);
        }

        //[HttpGet]
        //public async Task<ActionResult<List<ExpandoObject>>> GetAllUsers()
        //{
        //    try
        //    {
        //        var url = "https://dlsusermicroservice.azurewebsites.net/api/users";
        //        var content = await client.GetAsync(url);

        //        if (content.IsSuccessStatusCode)
        //        {
        //            var obj = await content.Content.ReadAsAsync<List<ExpandoObject>>();
        //            return obj;
        //        }
        //        else
        //            return Conflict("No entries in database");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpandoObject>> GetUser(int id)
        {
            try
            {
                //var url = string.Format("https://localhost:44320/api/users/{0}", id.ToString());
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id);
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

        [HttpGet]
        public ActionResult<ExpandoObject> GetUserEureka()
        {
            try
            {
                string content = eurekaClient.GetStringAsync("http://usermicroservice/api/users").Result;

                return null;
                //if (content.IsSuccessStatusCode)
                //{
                //    var obj = await content.Content.ReadAsAsync<ExpandoObject>();
                //    return obj;
                //}
                //else
                //    return Conflict("No entry with id " + id);
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
                if (user.Any(x => x.Key.ToLower() == "userid"))
                {
                    object id;
                    var delted = user.Remove(user.FirstOrDefault(x => x.Key.ToLower() == "userid").Key, out id);
                    if (!delted)
                        return Conflict("Some went wrong");
                }
                //var url = "https://localhost:44320/api/users/";
                var url = "https://dlsusermicroservice.azurewebsites.net/api/users/";
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
        public async Task<ActionResult<HttpResponseMessage>> UpdateUser(int id, [FromBody]ExpandoObject user)
        {
            try
            {
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id);
                var content = await client.PutAsJsonAsync<ExpandoObject>(url, user);
                if (content.IsSuccessStatusCode)
                    return content;
                else
                    return BadRequest("Id does not exist or is not an integer");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponseMessage>> DeleteUser(int id)
        {
            try
            {
                var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id);
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
