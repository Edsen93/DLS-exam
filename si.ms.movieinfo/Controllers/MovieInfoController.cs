using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ms.MovieInfo.Models;

namespace si.ms.movieinfo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieInfoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Movie>> Get()
        {
            try
            {
                MoviePersistence mp = new MoviePersistence();
                var movieList = mp.ReturnAllMovies();
                if (movieList.Count == 0)
                {
                    return Conflict("There is no movies in the database");
                }
                else
                {
                    return mp.ReturnAllMovies();
                }

            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);

            }
        }

        //search after movie title /api/movieinfo/batman
        [HttpGet("title/{title}")]
        public ActionResult<List<Movie>> Get(String title)
        {
            try
            {
                MoviePersistence mp = new MoviePersistence();
                var movielist = mp.SearchForMovie(title);
                if (movielist.Count <= 0)
                {
                    return Conflict("No movie was found");
                }
                return movielist;
            }
            catch (Exception ex)
            {

                return Conflict(ex.Message);
            }
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Movie>  Get(int id)
        {
            try
            {
                MoviePersistence mp = new MoviePersistence();
                Movie foundMovie = mp.findOneMovie(id);

                if (String.IsNullOrEmpty(foundMovie.title))
                {
                    var errorstr = String.Format("A movie with id {0} does not exits in the database", id);
                    return Conflict(errorstr);
                }
                else
                {
                    return foundMovie;


                }
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
                
            }
        }


        // POST: api/Default
        [HttpPost]
        public ActionResult<Movie> Post([FromBody]Movie value)
        {
            try
            {
                Movie m = value;
                MoviePersistence mp = new MoviePersistence();
                var currentyear = System.DateTime.Now.Year;
                if (m.releaseYear>= 1950 && m.releaseYear < currentyear)
                {
                    return mp.SaveMovie(m);
                }
                else
                {
                    var errorstr = String.Format("{0} Is not a valid year", m.releaseYear);
                    return Conflict(errorstr);
                }

            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
                
            }          
          
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public Movie Put(int id, [FromBody]Movie value)
        {
            MoviePersistence mp = new MoviePersistence();
            return mp.updateMovie(id, value);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public String Delete(int id)
        {
            MoviePersistence mp = new MoviePersistence();
            mp.deleteMovie(id);
            return String.Format("Movie with id{0} is delete", id);
        }
    }
}