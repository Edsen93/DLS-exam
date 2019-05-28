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
        public List<Movie> Get()
        {
            MoviePersistence mp = new MoviePersistence();
            return mp.SearchForMovie();
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public Movie Get(int id)
        {
            MoviePersistence mp = new MoviePersistence();

            return mp.findOneMovie(id);
        }


        // POST: api/Default
        [HttpPost]
        public Movie Post([FromBody]Movie value)
        {
            MoviePersistence mp = new MoviePersistence();

            return mp.SaveMovie(value);
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
        public void Delete(int id)
        {
            MoviePersistence mp = new MoviePersistence();
            mp.deleteMovie(id);
        }
    }
}