using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMicroServiceASP;
using UserMicroServiceASP.Models;

namespace DLSUserMicroService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var conn = new DBConn();
            var users = conn.GetUsers();
            return users;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var conn = new DBConn();
            var user = conn.GetUser(id);
            return user;
        }

        [HttpGet("{username}/{password}")]
        public ActionResult<User> Login(string username, string password)
        {
            var conn = new DBConn();
            var user = conn.Login(username, password);
            return user;
        }

        [HttpPost]
        public ActionResult<int> CreateUser([FromBody]User value)
        {
            var conn = new DBConn();
            var id = conn.CreateUser(value);

            return id;
        }

        [HttpPut("{id}")]
        public void UpdateUser(int id, [FromBody]User value)
        {
            var conn = new DBConn();
            conn.UpdateUser(id, value);
        }

        [HttpDelete("{id}")]
        public void DeleteUser(int id)
        {
            var conn = new DBConn();
            conn.DeleteUser(id);
        }
    }
}
