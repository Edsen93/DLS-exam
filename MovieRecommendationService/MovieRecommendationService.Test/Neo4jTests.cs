using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;

namespace MovieRecommendationService.Test
{
    [TestClass]
    public class BasicCoonnections
    {
        [TestMethod]
        public void Can_Connect_To_Neo4j()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

            // Read
            bool foundNeo4j = handler.CheckConnection();

            Assert.IsTrue(foundNeo4j);

        }


        [TestMethod]
        public void Can_Get_Recommendation()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

            // Read
            var result = handler.GetRecommendationOnUser(5);


            //dynamic test = result[0].Genre;


            Assert.IsNotNull(result);

        }
    }
}
