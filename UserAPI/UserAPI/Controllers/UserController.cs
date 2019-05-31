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
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            string content = await client.GetStringAsync(url);

            ExpandoObject user;
            if (!string.IsNullOrEmpty(content))
                user = JsonConvert.DeserializeObject<ExpandoObject>(content);
            else
                user = null;

            return user;
        }

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<ExpandoObject>> Login(string username, string password)
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}/{1}", username, password);
            string content = await client.GetStringAsync(url);

            ExpandoObject user;
            if (!string.IsNullOrEmpty(content))
                user = JsonConvert.DeserializeObject<ExpandoObject>(content);
            else
                user = null;

            return user;
        }

        [HttpPost]
        public async Task CreateUser([FromBody]ExpandoObject user)
        {          
            //var url = "https://localhost:44320/api/users/";
            var url = "https://dlsusermicroservice.azurewebsites.net/api/users/";
            var content = await client.PostAsJsonAsync<ExpandoObject>(url, user);
            if(content.IsSuccessStatusCode)
            {
                var msg = await content.Content.ReadAsAsync<ExpandoObject>();
                var list = msg.ToList();
                var id = new ExpandoObject();
                var result = id.TryAdd(list[2].Key, list[2].Value);
                if(result)
                {
                    //url = "https://localhost:44319/api/User";
                    url = "https://dlsrecommendmicroservice.azurewebsites.net/api/User";
                    content = await client.PostAsJsonAsync<ExpandoObject>(url, id);
                }             
            }
        }

        [HttpPut("{id}")]
        public async Task UpdateUser(int id, [FromBody]ExpandoObject user)  
        {
            var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
            var content = await client.PutAsJsonAsync<ExpandoObject>(url, user);
        }

        [HttpGet("{uId}/{mId}/{rating}")]
        public async Task PostUserRating(long uId, long mId, int rating)
        {
            //var url = string.Format("https://localhost:44319/api/User/ratemovie/{0}/{1}/{2}", uId, mId, rating);
            var url = string.Format("https://dlsrecommendmicroservice.azurewebsites.net/api/User/ratemovie/{0}/{1}/{2}", uId, mId, rating);
            var content = await client.GetStringAsync(url);
        }

        //[HttpDelete("{id}")]
        //public async Task DeleteUser(int id)
        //{
        //    var url = string.Format("https://dlsusermicroservice.azurewebsites.net/api/users/{0}", id.ToString());
        //    var content = await client.DeleteAsync(url);
        //}
    }
}
