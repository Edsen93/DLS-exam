using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRecommendationService.Test
{
    [TestClass]
    public class UserMovie_Relation_UnitTest
    {

        Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

        private static User CreateScenatio(Neo4jDatabaseHandler handler)
        {
            User user1 = new User() { UserId = 1 };
            User user2 = new User() { UserId = 2 };
            User user3 = new User() { UserId = 3 };
            User user4 = new User() { UserId = 4 };

            handler.CreateUser(user1);
            handler.CreateUser(user2);
            handler.CreateUser(user3);
            handler.CreateUser(user4);




            Movie movie1 = new Movie() { MovieId = 1, Title = "Title1", ReleaseYear = 2001, };
            Movie movie2 = new Movie() { MovieId = 2, Title = "Title2", ReleaseYear = 2002, };
            Movie movie3 = new Movie() { MovieId = 3, Title = "Title3", ReleaseYear = 2003, };
            Movie movie4 = new Movie() { MovieId = 4, Title = "Title4", ReleaseYear = 2004, };

            handler.CreateMovie(movie1);
            handler.CreateMovie(movie2);
            handler.CreateMovie(movie3);
            handler.CreateMovie(movie4);

            return user2;
        }



        [TestMethod]
        public void User_Can_Rate_Movie()
        {
            try
            {
                // Arrange
                User testUser = CreateScenatio(handler);

                long movie1 = 1;
                int rating1 = 4;
                long movie2 = 2;
                int rating2 = 3;

                //Act
                handler.SetUserMovieRating(testUser.UserId, movie1, rating1);
                handler.SetUserMovieRating(testUser.UserId, movie2, rating2);

                //Assert
                List<UserRating> foundRating = handler.GetUserMovieRatings(testUser.UserId);
                UserRating first = foundRating.FirstOrDefault(x => x.MovieId == movie1);
                UserRating second = foundRating.FirstOrDefault(x => x.MovieId == movie2);


                Assert.AreEqual(2, foundRating.Count, "number of relations is not correct");

                Assert.IsNotNull(first);
                Assert.AreEqual(movie1, first.MovieId);
                Assert.AreEqual(rating1, first.Rating);

                Assert.IsNotNull(second);
                Assert.AreEqual(movie2, second.MovieId);
                Assert.AreEqual(rating2, second.Rating);
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception: " + ex.ToString());
            }
            finally
            {
                // Cleanup
                handler.DeleteAllUsers();
                handler.DeleteAllMovies();
            }
        }


        [TestMethod]
        public void User_can_update_Rating()
        {
            try
            {
                // Arrange
                User testUser = CreateScenatio(handler);

                long movie1 = 1;
                int rating1 = 4;
                int rating2 = 3;

                //Act
                handler.SetUserMovieRating(testUser.UserId, movie1, rating1);
                handler.SetUserMovieRating(testUser.UserId, movie1, rating2);

                //Assert
                List<UserRating> foundRating = handler.GetUserMovieRatings(testUser.UserId);
                UserRating returned = foundRating.FirstOrDefault(x => x.MovieId == movie1);


                Assert.AreEqual(2, foundRating.Count, "number of relations is not correct");

                Assert.IsNotNull(returned);
                Assert.AreEqual(movie1, returned.MovieId);
                Assert.AreEqual(rating2, returned.Rating);

            }
            catch (Exception ex)
            {
                Assert.Fail("Exception: " + ex.ToString());
            }
            finally
            {
                // Cleanup
                handler.DeleteAllUsers();
                handler.DeleteAllMovies();
            }
        }


        [TestMethod]
        public void User_can_Delete_Rating()
        {
            try
            {
                // Arrange
                User testUser = CreateScenatio(handler);

                long movie1 = 1;
                int rating1 = 4;

                handler.SetUserMovieRating(testUser.UserId, movie1, rating1);

                //Act
                handler.DeleteUserRatingMovie(testUser.UserId, movie1);

                //Assert
                List<UserRating> foundRating = handler.GetUserMovieRatings(testUser.UserId);
                UserRating returned = foundRating.FirstOrDefault(x => x.MovieId == movie1);


                Assert.AreEqual(0, foundRating.Count, "number of relations is not correct");

                Assert.IsNull(returned);

            }
            catch (Exception ex)
            {
                Assert.Fail("Exception: " + ex.ToString());
            }
            finally
            {
                // Cleanup
                handler.DeleteAllUsers();
                handler.DeleteAllMovies();
            }
        }


        [TestMethod]
        public void User_Deleting_Rating_does_not_effect_other()
        {
            try
            {
                // Arrange
                User testUser = CreateScenatio(handler);

                long movie1 = 1;
                int rating1 = 4;
                long movie2 = 2;
                int rating2 = 3;

                handler.SetUserMovieRating(testUser.UserId, movie1, rating1);
                handler.SetUserMovieRating(testUser.UserId, movie2, rating2);

                //Act
                handler.DeleteUserRatingMovie(testUser.UserId, movie1);

                //Assert
                List<UserRating> foundRating = handler.GetUserMovieRatings(testUser.UserId);
                UserRating returned = foundRating.FirstOrDefault(x => x.MovieId == movie2);


                Assert.AreEqual(1, foundRating.Count, "number of relations is not correct");

                Assert.IsNotNull(returned);
                Assert.AreEqual(movie2, returned.MovieId);
                Assert.AreEqual(rating2, returned.Rating);

            }
            catch (Exception ex)
            {
                Assert.Fail("Exception: " + ex.ToString());
            }
            finally
            {
                // Cleanup
                handler.DeleteAllUsers();
                handler.DeleteAllMovies();
            }
        }



        [TestMethod]
        public void User_Rating_Movie_does_not_duplicate_it()
        {
            try
            {
                // Arrange
                User testUser = CreateScenatio(handler);

                long movie1 = 1;
                int rating1 = 4;
                long movie2 = 2;
                int rating2 = 3;

                var moviesBefore = handler.GetAllMovies();
                var userBefore = handler.GetAllUsers();

                //Act
                handler.SetUserMovieRating(testUser.UserId, movie1, rating1);
                handler.SetUserMovieRating(testUser.UserId, movie2, rating2);

                //Assert
                List<UserRating> foundRating = handler.GetUserMovieRatings(testUser.UserId);
                UserRating first = foundRating.FirstOrDefault(x => x.MovieId == movie1);
                UserRating second = foundRating.FirstOrDefault(x => x.MovieId == movie2);

                var moviesAfter = handler.GetAllMovies();
                var userAfter = handler.GetAllUsers();

                Assert.AreEqual(moviesBefore.Count, moviesAfter.Count);
                Assert.AreEqual(userBefore.Count, userAfter.Count);

            }
            catch (Exception ex)
            {
                Assert.Fail("Exception: " + ex.ToString());
            }
            finally
            {
                // Cleanup
                handler.DeleteAllUsers();
                handler.DeleteAllMovies();
            }
        }

    }
}