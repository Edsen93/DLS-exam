using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RecommendationRESTService.Models
{
    public class Neo4jHandler
    {
        public static Neo4jDatabaseHandler GetHandler()
        {
            //string address = ConfigurationManager.AppSettings["Neo4jAddress"];
            //string user = ConfigurationManager.AppSettings["Neo4jUser"];
            //string password = ConfigurationManager.AppSettings["Neo4jPassword"];


            string address = "https://cyan-icie-centers-kirstin.graphstory.services/db/data/";
            string user = "cyan_icie_centers_kirstin";
            string password = "pyXye9rgiTmBeuQ8HUaM";


            return new Neo4jDatabaseHandler(address, user, password);
        }
    }
}