using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationService.Test
{
    [TestClass]
    public class Recommendations_UnitTest
    {
        [TestMethod]
        public void Should_Not_return_recommend_when_userid_is_not_found()
        {
            string address = "https://cyan-icie-centers-kirstin.graphstory.services/db/data/";
            string user = "cyan_icie_centers_kirstin";
            string password = "pyXye9rgiTmBeuQ8HUaM";

            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler(address,user,password);

            var result = handler.GetRecommendationOnUser(-1);

            Assert.AreEqual(0, result.Count);

        }

    }
}
