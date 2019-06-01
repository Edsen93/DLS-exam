using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMicroServiceASP;
using UserMicroServiceASP.Models;
using System.Net.Http;

namespace DLSUserMicroService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<HttpResponseMessage> GetAllUsers()
        {
            var conn = new DBConn();
            var users = conn.GetUsers();
            if (users.Count <= 0)
                return Conflict("There are no users in the database");
            else
                return Ok(users)
        }

        [HttpGet("{id}")]
        public ActionResult<HttpResponseMessage> GetUser(int id)
        {
            var conn = new DBConn();
            var user = conn.GetUser(id);
            if (user.UserId <= 0 || string.IsNullOrWhiteSpace(user.Username))
                return Conflict("User with id " + id + " does not exist");
            else
                return Ok(user)
        }

        [HttpGet("{username}/{password}")]
        public ActionResult<HttpResponseMessage> Login(string username, string password)
        {
            var conn = new DBConn();
            var user = conn.Login(username, password);
            if (user.UserId <= 0 || string.IsNullOrWhiteSpace(user.Username))
                return Conflict("Wrong username or password");
            else
                return Ok(user);
        }

        [HttpPost]
        public ActionResult<HttpResponseMessage> CreateUser([FromBody]User value)
        {
            var conn = new DBConn();
            var user = conn.CreateUser(value);
            if (user != null)
                return Ok(user);

            return Conflict("Username exist");
        }

        [HttpPut("{id}")]
        public ActionResult<HttpRequestMessage> UpdateUser(int id, [FromBody]User value)
        {
            var conn = new DBConn();
            var affectedrows = conn.UpdateUser(id, value);
            if (affectedrows > 0)
                return Ok("Row(s) updated");
            else
                return Conflict("Id does not exist");
        }

        [HttpDelete("{id}")]
        public ActionResult<HttpRequestMessage> DeleteUser(int id)
        {
            var conn = new DBConn();
            var affectedrows = conn.DeleteUser(id);
            if (affectedrows > 0)
                return Ok("Row(s) deleted");
            else
                return Conflict("Id does not exist");
        }
    }
}
