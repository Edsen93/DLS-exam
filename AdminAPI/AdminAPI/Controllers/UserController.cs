using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<string>> GetAllUsers()
        {
            var url = "https://dlsusermicroservice.azurewebsites.net/api/users";
            string content = await client.GetStringAsync(url);
            return content;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetUser(int id)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            string content = await client.GetStringAsync(url);
            return content;
        }

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}/{1}", username, password);
            string content = await client.GetStringAsync(url);
            return content;
        }

        [HttpPost]
        public async Task CreateUser([FromBody]string user)
        {
            var url = "https://dlsusermicroservice.azurewebsites.net/api/users/";
            var content = await client.PostAsJsonAsync<string>(url, user);
        }

        [HttpPut("{id}")]
        public async Task UpdateUser(int id, [FromBody]string user)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            var content = await client.PutAsJsonAsync<string>(url, user);
        }

        [HttpDelete("{id}")]
        public async Task DeleteUser(int id)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            var content = await client.DeleteAsync(url);
        }
    }
}
