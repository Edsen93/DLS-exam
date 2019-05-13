using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using RecommendationRESTService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecommendationRESTService.Controllers
{
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api

    //[RoutePrefix("Movie")]
    public class MovieController : ApiController
    {
        List<Movie> _movies = new List<Movie>()
        {
            new Movie() { MovieId = 1, ReleaseYear = 1998, Title = "Movie1" },
            new Movie() { MovieId = 2, ReleaseYear = 1992, Title = "Movie2" },
        };

        public MovieController() { }

        public MovieController(List<Movie> products)
        {
            this._movies = products;
        }


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
            Movie result = null;
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                result = handler.GetMovie(id);
            }
            catch (Exception)
            {
            }
            
            return result;
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




    //public class NewMovieController : ApiController
    //{
    //    List<Movie> _movies = new List<Movie>()
    //    {
    //        new Movie() { MovieId = 1, ReleaseYear = 1998, Title = "Movie1" },
    //        new Movie() { MovieId = 2, ReleaseYear = 1992, Title = "Movie2" },
    //    };

    //    public NewMovieController() { }

    //    public NewMovieController(List<Movie> products)
    //    {
    //        this._movies = products;
    //    }

    //    public IEnumerable<Movie> GetAllMovies()
    //    {
    //        return _movies;
    //    }

    //    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    //    {
    //        return await Task.FromResult(GetAllMovies());
    //    }

    //    public IHttpActionResult GetMovie(int id)
    //    {
    //        var product = _movies.FirstOrDefault((p) => p.MovieId == id);
    //        if (product == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(product);
    //    }

    //    public async Task<IHttpActionResult> GetProductAsync(int id)
    //    {
    //        return await Task.FromResult(GetMovie(id));
    //    }
    //}
}
