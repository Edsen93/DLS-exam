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
    public class User_UnitTest
    {
        

        [TestMethod]
        public void Can_Create_one_User()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

            Random rnd = new Random();
            long userId = rnd.Next(1, 50000); 

            User user = new User() { UserId = userId };

            // count
            int preCount = handler.GetAllUsers().Count;

            // Create
            handler.CreateUser(user);

            // Read
            int resultCount = handler.GetAllUsers().Count;
            User foundUser = handler.GetUser(userId);

            // Delete
            handler.DeleteUser(userId);

            // count
            int postCount = handler.GetAllUsers().Count;

            
            
            Assert.AreEqual(0, preCount,"Database contains some data");

            Assert.AreEqual(1, resultCount, "the correct ammount of users was not found");
            Assert.AreNotSame(user, foundUser, "the found user was the same object");
            Assert.AreEqual(user.UserId, foundUser.UserId);
            
            Assert.AreEqual(0, postCount, "User was not deleted");
        }
        
        [TestMethod]
        public void Can_Create_4_User()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();
            

            User user1 = new User() { UserId = 1 };
            User user2 = new User() { UserId = 2 };
            User user3 = new User() { UserId = 3 };
            User user4 = new User() { UserId = 4 };

            // count
            int preCount = handler.GetAllUsers().Count;

            // Create
            handler.CreateUser(user1);
            handler.CreateUser(user2);
            handler.CreateUser(user3);
            handler.CreateUser(user4);

            // Read
            List<User> foundUsers = handler.GetAllUsers();

            // Delete
            handler.DeleteAllUsers();

            // count
            int postCount = handler.GetAllUsers().Count;



            Assert.AreEqual(0, preCount, "Database was not cleared before the test");

            Assert.AreEqual(4, foundUsers.Count, "The correct ammount of users was not found");
            Assert.IsTrue(foundUsers.Any(x => x.UserId == user1.UserId), "1 user was not found");
            Assert.IsTrue(foundUsers.Any(x => x.UserId == user2.UserId), "2 user was not found");
            Assert.IsTrue(foundUsers.Any(x => x.UserId == user3.UserId), "3 user was not found");
            Assert.IsTrue(foundUsers.Any(x => x.UserId == user4.UserId), "4 user was not found");
            
            Assert.AreEqual(0, postCount, "Data was not cleared after test");
        }

        [TestMethod]
        public void Can_Get_User_Count()
        {
            Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();


            User user1 = new User() { UserId = 1 };
            User user2 = new User() { UserId = 2 };
            User user3 = new User() { UserId = 3 };
            User user4 = new User() { UserId = 4 };

            // count
            int preCount = handler.GetAllUsers().Count;

            // Create
            handler.CreateUser(user1);
            handler.CreateUser(user2);
            handler.CreateUser(user3);
            handler.CreateUser(user4);

            // Read
            int testCount = handler.GetAllUsers().Count;

            // Clean up
            handler.DeleteAllUsers();

            Assert.AreEqual(0, preCount, "Database was not cleared before the test");
            
            Assert.AreEqual(4, testCount);
        }

    }
}
