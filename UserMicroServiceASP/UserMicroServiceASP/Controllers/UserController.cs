using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserMicroServiceASP.Models;

namespace UserMicroServiceASP.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {

        [HttpGet]
        [Route("")]
        public IEnumerable<User> GetAllUsers()
        {
            var conn = new DBConn();
            var users = conn.GetUsers();
            return users;
        }

        [HttpGet]
        [Route("{id:int}")]
        public User GetUser(int id)
        {
            var conn = new DBConn();
            var user = conn.GetUser(id);
            return user;
        }

        [HttpGet]
        [Route("{username}/{password}")]
        public User Login(string username, string password)
        {
            var conn = new DBConn();
            var user = conn.Login(username, password);
            return user;
        }

        [HttpPost]
        [Route("")]
        public void CreateUser([FromBody]User value)
        {   
            var conn = new DBConn();
            conn.CreateUser(value);
        }

        [HttpPut]
        [Route("{id:int}")]
        public void UpdateUser(int id, [FromBody]User value)
        {
            var conn = new DBConn();
            conn.UpdateUser(id, value);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void DeleteUser(int id)
        {
            var conn = new DBConn();
            conn.DeleteUser(id);
        }
    }
}
