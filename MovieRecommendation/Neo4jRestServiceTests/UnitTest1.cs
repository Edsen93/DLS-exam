using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neo4jRestServiceTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var testProducts = GetTestProducts();
            var controller = new MovieController(testProducts);

            var result = controller.GetAllProducts() as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
        }
    }
}
