using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using Xunit;

namespace TestAdminApi
{
    public class UnitTest1
    {
        HttpClient client;

        [Fact]
        public async void TestConnection()
        {
            client = new HttpClient();
            var check = await client.GetAsync(string.Format("http://dlsadminapi.azurewebsites.net/api/users/{0}", 4));

            Assert.True(check.IsSuccessStatusCode);
        }

        [Fact]
        public async void Can_Create_one_User()
        {
            client = new HttpClient();
            // count
            var url = "http://dlsadminapi.azurewebsites.net/api/users/";
            var jsonObject = await client.GetStringAsync(url);
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);

            // isAdmin, username, userId, password, email
            var obj = new ExpandoObject();
            obj.TryAdd("isadmin", true);
            obj.TryAdd("username", "basdamgaard");
            obj.TryAdd("password", "1234567890");
            obj.TryAdd("email", "seb@dam.com");

            // Create
            var content = await client.PostAsJsonAsync<ExpandoObject>(url, obj);

            // Read
            jsonObject = await client.GetStringAsync(url);
            var list = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);

            // Delete
            //handler.DeleteUser(userId);

            //// count
            //int postCount = handler.GetAllUsers().Count;



            //Assert.AreEqual(0, preCount, "Database contains some data");

            //Assert.AreEqual(1, resultCount, "the correct ammount of users was not found");
            //Assert.AreNotSame(user, foundUser, "the found user was the same object");
            //Assert.AreEqual(user.UserId, foundUser.UserId);

            //Assert.AreEqual(0, postCount, "User was not deleted");
        }
    }
}
