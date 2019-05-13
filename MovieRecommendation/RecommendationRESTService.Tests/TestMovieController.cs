using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRecommendationLibrary.Model;
using RecommendationRESTService.Controllers;

namespace RecommendationRESTService.Tests
{
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/unit-testing-with-aspnet-web-api
    [TestClass]
    public class TestMovieController
    {
        [TestMethod]
        public void GetAllMovies_ShouldReturnAllMovies()
        {
            List<Movie> _movies = new List<Movie>()
            {
                new Movie() { MovieId = 1, ReleaseYear = 1998, Title = "Movie1" },
                new Movie() { MovieId = 2, ReleaseYear = 1992, Title = "Movie2" },
            };
            var controller = new MovieController(_movies);

            var result = controller.GetAllProducts() as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
        }
    }
}
