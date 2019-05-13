using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Neo4jRESTService.Models
{
    public class Neo4jHandler
    {
        public static Neo4jDatabaseHandler GetHandler()
        {
            string address = ConfigurationManager.AppSettings["Neo4jAddress"];
            string user = ConfigurationManager.AppSettings["Neo4jUser"];
            string password = ConfigurationManager.AppSettings["Neo4jPassword"];

            return new Neo4jDatabaseHandler(address, user, password);
        }
    }
}