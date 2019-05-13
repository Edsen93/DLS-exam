using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;


using System.Diagnostics;
using System.Linq;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using MovieRecommendationLibrary.Model;
using System.Collections.Generic;


//using RecommenderLibrary;

namespace Neo4jTests
{
    [TestClass]
    public class Movie_UnitTest
    {
        Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

        //[TestInitialize]
        //public void Setup()
        //{
        //}

        //[TestCleanup]
        //public void Cleanup()
        //{
        //    //This is just to slow
        //    handler.DeleteAllGenres();
        //    handler.DeleteAllMovies();
        //    handler.DeleteAllUsers();
        //}



        [TestMethod]
        public void Can_Create_Movie()
        {
            try
            { 
                //Arrange
                Movie movie = GenerateMovie();

                // Act
                handler.CreateMovie(movie);

                // Assert
                Movie foundMovie = handler.GetMovie(movie.MovieId);
            
                Assert.AreNotSame(movie, foundMovie, "the found movie was the same object");
                Assert.IsNotNull(foundMovie);
                Assert.AreEqual(movie.MovieId, foundMovie.MovieId);
                Assert.AreEqual(movie.Title, foundMovie.Title);
                Assert.AreEqual(movie.ReleaseYear, foundMovie.ReleaseYear);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllMovies();
            }
        }

        [TestMethod]
        public void Can_Delete_Movie()
        {
            try
            {
                //Arrange
                Movie movie = GenerateMovie();

                // Create
                handler.CreateMovie(movie);
                handler.DeleteMovie(movie.MovieId);

                Movie foundMovie = handler.GetMovie(movie.MovieId);
            
                Assert.IsNull(foundMovie);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllMovies();
            }
        }

        [TestMethod]
        public void Delete_does_not_Delete_other_Movie()
        {
            try
            {
                //Arrange
                Movie movie = GenerateMovie();
                Movie movie2 = GenerateMovie();

                // Act
                handler.CreateMovie(movie);
                handler.CreateMovie(movie2);
                handler.DeleteMovie(movie.MovieId);


                //Assert
                Movie foundMovie = handler.GetMovie(movie.MovieId);
                Movie foundMovie2 = handler.GetMovie(movie2.MovieId);
                List<Movie> foundMovieList = handler.GetAllMovies();

                Assert.IsNull(foundMovie);
                Assert.IsNotNull(foundMovie2);
                Assert.AreEqual(movie2.MovieId, foundMovie2.MovieId);
                Assert.AreEqual(movie2.Title, foundMovie2.Title);
                Assert.AreEqual(movie2.ReleaseYear, foundMovie2.ReleaseYear);
                Assert.AreEqual(1, foundMovieList.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllMovies();
            }
        }



        [TestMethod]
        public void Can_Create_4_Movies()
        {

            try
            {
                //Arrange
                Movie movie1 = new Movie() { MovieId = 1, Title = "Title1", ReleaseYear = 1921 };
                Movie movie2 = new Movie() { MovieId = 2, Title = "Title2", ReleaseYear = 1922 };
                Movie movie3 = new Movie() { MovieId = 3, Title = "Title3", ReleaseYear = 1923 };
                Movie movie4 = new Movie() { MovieId = 4, Title = "Title4", ReleaseYear = 1924 };
            

                // Act
                handler.CreateMovie(movie1);
                handler.CreateMovie(movie2);
                handler.CreateMovie(movie3);
                handler.CreateMovie(movie4);

                // Assert
                List<Movie> foundMovies = handler.GetAllMovies();
           
            
                Assert.IsTrue(foundMovies.Any(x => x.MovieId == movie1.MovieId), "1 movie was not found");
                Assert.IsTrue(foundMovies.Any(x => x.MovieId == movie1.MovieId), "2 movie was not found");
                Assert.IsTrue(foundMovies.Any(x => x.MovieId == movie1.MovieId), "3 movie was not found");
                Assert.IsTrue(foundMovies.Any(x => x.MovieId == movie1.MovieId), "4 movie was not found");
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllMovies();
            }
        }

        [TestMethod]
        public void Can_Delete_all_Movies()
        {

            try
            {
                //Arrange
                Movie movie1 = new Movie() { MovieId = 1, Title = "Title1", ReleaseYear = 1921 };
                Movie movie2 = new Movie() { MovieId = 2, Title = "Title2", ReleaseYear = 1922 };
                Movie movie3 = new Movie() { MovieId = 3, Title = "Title3", ReleaseYear = 1923 };
                Movie movie4 = new Movie() { MovieId = 4, Title = "Title4", ReleaseYear = 1924 };

                
                handler.CreateMovie(movie1);
                handler.CreateMovie(movie2);
                handler.CreateMovie(movie3);
                handler.CreateMovie(movie4);
            
                // Act
                handler.DeleteAllMovies();

                // Assert
                int postCount = handler.GetAllMovies().Count;
            
                Assert.AreEqual(0, postCount, "Data was not cleared after test");
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllMovies();
            }
        }



        [TestMethod]
        public void Can_Create_Genre()
        {

            try
            {
                // Arrange
                string gName = "TestGenre";
                List<Genre> genreList = new List<Genre>() {  new Genre() { GenreName = gName }}; 
                Genre genre = new Genre() { GenreName = gName };
                Movie movie = new Movie() {  MovieId = 200 };

                //Act
                handler.CreateMovie(movie);
                handler.SetMovieGenre(200, genreList);


                //Assert
                List<Genre> result = handler.GetGenres();
            
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(gName, result.First().GenreName);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllGenres();
                handler.DeleteAllMovies();
            }
        }


        [TestMethod]
        public void Can_Create_multible_Genre()
        {
            try
            {
                // Arrange
                string gName1 = "TestGenre1";
                string gName2 = "TestGenre2";
                string gName3 = "TestGenre3";
           
                Genre genre1 = new Genre() { GenreName = gName1 };
                Genre genre2 = new Genre() { GenreName = gName2 };
                Genre genre3 = new Genre() { GenreName = gName3 };
                List<Genre> genreList = new List<Genre>() { genre1, genre2, genre3 };
                Movie movie = new Movie() { MovieId = 200 };


                //Act
                handler.CreateMovie(movie);
                handler.SetMovieGenre(200, genreList);

                //Assert
                List<Genre> result = handler.GetGenres();

                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Any(x => x.GenreName == gName1));
                Assert.IsTrue(result.Any(x => x.GenreName == gName2));
                Assert.IsTrue(result.Any(x => x.GenreName == gName3));
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllGenres();
                handler.DeleteAllMovies();
            }
        }

        [TestMethod]
        public void Will_merge_Genre_on_duplication()
        {
            try
            {
                // Arrange
                string gName = "TestGenre";
                Genre genre1 = new Genre() { GenreName = gName };
                Genre genre2 = new Genre() { GenreName = gName };


                Movie movie = new Movie() { MovieId = 200 };
                List<Genre> genreList = new List<Genre>() { genre1, genre2 };

                //Act
                handler.CreateMovie(movie);
                handler.SetMovieGenre(200, genreList);

                //Assert
                List<Genre> result = handler.GetGenres();
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(gName, result.First().GenreName);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllGenres();
                handler.DeleteAllMovies();
            }
        }


        [TestMethod]
        public void Will_merge_movie_when_adding_rating()
        {
            try
            {
                // Arrange
                string gName = "TestGenre";
                Genre genre1 = new Genre() { GenreName = gName };
                Genre genre2 = new Genre() { GenreName = gName };


                Movie movie = new Movie() { MovieId = 200 };
                List<Genre> genreList = new List<Genre>() { genre1, genre2 };

                //Act
                handler.CreateMovie(movie);
                handler.SetMovieGenre(200, genreList);

                //Assert
                List<Genre> result = handler.GetGenres();
                List<Movie> movieResult = handler.GetAllMovies();

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(gName, result.First().GenreName);


                Assert.AreEqual(1, movieResult.Count);
                Assert.AreEqual(movie.Title, movieResult.First().Title);
            }
            catch (Exception ex)
            {
                Assert.Fail("Threw Exception: " + ex.ToString());
            }
            finally
            {
                handler.DeleteAllGenres();
                handler.DeleteAllMovies();
            }
        }



        private static Random rnd = new Random();

        private static Movie GenerateMovie()
        {

            return new Movie()
            {
                MovieId = rnd.Next(1, 50000),
                Title = rnd.Next(1, 50000).ToString(),
                ReleaseYear = rnd.Next(1, 50000),
            };
        }


    }
}
