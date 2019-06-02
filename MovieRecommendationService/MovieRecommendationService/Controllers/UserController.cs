using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using RecommendationRESTService.Models;

namespace MovieRecommendationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                return handler.GetAllUsers();

            }
            catch (Exception)
            {
            }
            return null;
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<User> Get(long id)
        {
            User result = null;
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                result = handler.GetUser(id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            if (result is null)
                return NotFound();
            else
                return result;
        }

        // POST: api/User
        [HttpPost]
        public ActionResult Post([FromBody] User value)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                handler.CreateUser(value);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet("ratemovie/{uId}/{mId}/{rating}")]
        public ActionResult PostUserRating(long uId, long mId, int rating)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                handler.SetUserMovieRating(uId,mId,rating);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        //// PUT: api/User/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                handler.DeleteUser(id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return Ok();

        }
    }
}
