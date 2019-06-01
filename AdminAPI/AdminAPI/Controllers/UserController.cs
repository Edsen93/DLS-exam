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

        [HttpGet]
        public async Task<ActionResult<List<ExpandoObject>>> GetAllUsers()
        {
            var url = "https://dlsusermicroservice.azurewebsites.net/api/users";
            var content = await client.GetAsync(url);

            if (content.IsSuccessStatusCode)
            {
                var obj = await content.Content.ReadAsAsync<List<ExpandoObject>>();
                return obj;
            }
            else
                return Conflict("No entries in database");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpandoObject>> GetUser(int id)
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

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<ExpandoObject>>Login(string username, string password)
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

        [HttpPost]
        public async Task<ActionResult<HttpResponseMessage>> CreateUser([FromBody]ExpandoObject user)
        {          
            //var url = "https://localhost:44320/api/users/";
            var url = "https://dlsusermicroservice.azurewebsites.net/api/users/";
            var content = await client.PostAsJsonAsync<ExpandoObject>(url, user);
            if (content.IsSuccessStatusCode)
            {
                var msg = await content.Content.ReadAsAsync<ExpandoObject>();
                var id = new ExpandoObject();
                var result = id.TryAdd(msg.ElementAt(2).Key, msg.ElementAt(2).Value);
                if (result)
                {
                    //url = "https://localhost:44319/api/User";
                    url = "https://dlsrecommendmicroservice.azurewebsites.net/api/User";
                    content = await client.PostAsJsonAsync<ExpandoObject>(url, id);
                    return content;
                }
                else
                    return BadRequest();
            }
            else
                return Conflict("User exist");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HttpResponseMessage>> UpdateUser(int id, [FromBody]ExpandoObject user)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            var content = await client.PutAsJsonAsync<ExpandoObject>(url, user);
            return content;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponseMessage>> DeleteUser(int id)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            var content = await client.DeleteAsync(url);
            return content;
        }
    }
}
