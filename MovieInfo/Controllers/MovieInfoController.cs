using ms.MovieInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ms.MovieInfo.Controllers
{
    public class MovieInfoController : ApiController
    {
        // GET: api/MovieInfo
        // FIND ALL MOVIES
        public List<Movie> Get()
        {
            MoviePersistence mp = new MoviePersistence();
            return mp.SearchForMovie();
        }

        // GET: api/MovieInfo/5
        public String Get(int id)
        {
            MoviePersistence mp = new MoviePersistence();
            //return mp.findOneMovie(id);
            return "bob";
        }

        // POST: api/MovieInfo
        public Movie Post([FromBody]Movie value)
        {
            MoviePersistence mp = new MoviePersistence();
 
            return mp.SaveMovie(value);
        }

        // PUT: api/MovieInfo/5
        public Movie Put(int id, [FromBody]Movie value)
        {
            MoviePersistence mp = new MoviePersistence();
            return mp.updateMovie(id, value);
            
        }

        // DELETE: api/MovieInfo/5
        public void Delete(int id)
        {
            MoviePersistence mp = new MoviePersistence();
            mp.deleteMovie(id);
        }

        // DELETE: api/MovieInfo/1/2/3
        public String Get(int id, int news)
        {
            return id.ToString() + news.ToString();
        }
    }
}
