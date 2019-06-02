using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMicroServiceASP;
using UserMicroServiceASP.Models;
using System.Net.Http;
using System.Net;

namespace DLSUserMicroService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            try
            {
                var conn = new DBConn();
                var users = conn.GetUsers();
                if (users.Count <= 0)
                    return Conflict("There are no users in the database");
                else
                    return users;
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var conn = new DBConn();
                var user = conn.GetUser(id);
                if (user.UserId <= 0 || string.IsNullOrWhiteSpace(user.Username))
                    return Conflict("User with id " + id + " does not exist");
                else
                    return user;
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("{username}/{password}")]
        public ActionResult<User> Login(string username, string password)
        {
            try
            {
                var conn = new DBConn();
                var user = conn.Login(username, password);
                if (user.UserId <= 0 || string.IsNullOrWhiteSpace(user.Username))
                    return Conflict("Wrong username or password");
                else
                    return user;
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody]User value)
        {
            try
            {
                var conn = new DBConn();
                var user = conn.CreateUser(value);
                if (user != null)
                    return user;

                return Conflict("Username exist");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<HttpRequestMessage> UpdateUser(int id, [FromBody]User value)
        {
            try
            {
                var conn = new DBConn();
                var affectedrows = conn.UpdateUser(id, value);
                if (affectedrows > 0)
                    return Ok("Row(s) updated");
                else
                    return Conflict("Id does not exist");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<HttpRequestMessage> DeleteUser(int id)
        {
            try
            {
                var conn = new DBConn();
                var affectedrows = conn.DeleteUser(id);
                if (affectedrows > 0)
                    return Ok("Row(s) deleted");
                else
                    return Conflict("Id does not exist");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
