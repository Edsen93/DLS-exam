using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using MovieRecommendationLibrary.Model;
using System.Collections.Generic;


//using RecommenderLibrary;

namespace Neo4jTests
{
    [TestClass]
    public class Connection_UnitTest
    {

        [TestMethod]
        public void Can_Connect_To_Neo4j()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();
            
            // Read
            bool foundNeo4j = handler.CheckConnection();
            
            Assert.IsTrue(foundNeo4j);
            
        }
        

    }
}
