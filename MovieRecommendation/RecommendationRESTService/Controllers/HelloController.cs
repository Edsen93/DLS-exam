using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RecommendationRESTService.Controllers
{
    public class HelloController : ApiController
    {
        public string Get()
        {
            return "Hello world, this is RecommendationREST api";
        }
    }
}
