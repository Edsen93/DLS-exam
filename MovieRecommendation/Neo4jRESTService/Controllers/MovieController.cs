using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using Neo4jRESTService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Neo4jRESTService.Controllers
{
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api

    //[RoutePrefix("Movie")]
    public class MovieController : ApiController
    {
        //[HttpGet]
        //[Route("{username}/{password}")]
        //public IEnumerable<Movie> Get(string username, string password)

        //[HttpGet]
        //[Route("{genre}")]
        //public IEnumerable<Movie> Get(string genre)


        //[HttpGet]
        //[Route("{id:int}")]
        //public IEnumerable<Movie> Get(int id)
        //{
        //    return new List<Movie>()
        //    {
        //        new Movie() { MovieId = id,  Title = "Test" }
        //    };
        //}

        // GET: api/Movie
        public IEnumerable<Movie> Get()
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                return handler.GetAllMovies();
            }
            catch (Exception)
            {
            }
            return null;
        }

        // GET: api/Movie/5
        public Movie Get(long id)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                return handler.GetMovie(id);
            }
            catch (Exception)
            {
            }
            return null;
        }

        // POST: api/Movie
        public void Post([FromBody]Movie value)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                handler.CreateMovie(value);
            }
            catch (Exception)
            {
            }
        }

        //// PUT: api/Movie/5
        //public void Put(long id, [FromBody]Movie value)
        //{
        //    Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
        //    //handler.UpdateMovie(id, value);
        //}

        // DELETE: api/Movie/5
        public void Delete(long id)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                handler.DeleteMovie(id);
            }
            catch (Exception)
            {
            }
        }
    }
}
