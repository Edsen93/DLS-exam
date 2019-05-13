using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using RecommendationRESTService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RecommendationRESTService.Controllers
{
    public class RecommendationController : ApiController
    {
        //// GET: api/Recommendation
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Recommendation/5
        public IEnumerable<MovieRecommendation> Get(long id)
        {
            try
            {
                Neo4jDatabaseHandler handler = Neo4jHandler.GetHandler();
                return handler.GetRecommendationOnMovie(id);
            }
            catch (Exception)
            {
            }
            return null;
        }

        //// POST: api/Recommendation
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Recommendation/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Recommendation/5
        //public void Delete(int id)
        //{
        //}
    }
}
