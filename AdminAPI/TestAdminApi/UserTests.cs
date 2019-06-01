using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace TestAdminApi
{
    public class UserTest
    {
        HttpClient client;

        [Fact]
        public void TestConnection()
        {
            client = new HttpClient();
            var check = client.GetAsync(string.Format("http://dlsadminapi.azurewebsites.net/api/users/{0}", 4)).Result;

            Assert.True(check.IsSuccessStatusCode);
        }

        [Fact]
        public  void TestCRDUser()
        {
            client = new HttpClient();
            // count
            var url = "http://dlsadminapi.azurewebsites.net/api/users/";
            var jsonObject = client.GetStringAsync(url).Result;
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            var obj = new ExpandoObject();
            obj.TryAdd("isAdmin", true);
            obj.TryAdd("username", "basdamgaard");
            obj.TryAdd("password", "1234567890");
            obj.TryAdd("email", "seb@dam.com");

            // Create
            jsonObject = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = client.PostAsync(url, stringcontent).Result;

            // Read
            jsonObject = client.GetStringAsync(url).Result;
            var newcount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;
            var userjson = client.GetStringAsync(url + "/basdamgaard/1234567890").Result;
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(userjson);

            // Delete
            var contentdel = client.DeleteAsync(url + '/' + newObject.ElementAt(2).Value).Result;

            //// count
            jsonObject = client.GetStringAsync(url).Result;
            var postCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            Assert.False(preCount == newcount, "Object not added");
            Assert.True(content.IsSuccessStatusCode, "Something went wrong when posting");
            Assert.True(contentdel.IsSuccessStatusCode, "Something went wrong when deleting");
            Assert.True(string.Equals(newObject.ElementAt(1).Value, obj.ElementAt(1).Value), "Object could not be found after being inserted");
            Assert.True(preCount == postCount, "Object not deleted");
        }

        [Fact]
        public void TestCRDMultipleUsers()
        {
            client = new HttpClient();
            // count
            var url = "http://dlsadminapi.azurewebsites.net/api/users";
            var jsonObject = client.GetStringAsync(url).Result;
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;


            // Create first 
            var obj1 = new ExpandoObject();
            obj1.TryAdd("isAdmin", true);
            obj1.TryAdd("username", "basdamgaard");
            obj1.TryAdd("password", "1234567890");
            obj1.TryAdd("email", "seb@dam.com");
            jsonObject = JsonConvert.SerializeObject(obj1);
            var stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var contentone = client.PostAsync(url, stringcontent).Result;


            // Create second
            var obj2 = new ExpandoObject();
            obj2.TryAdd("isAdmin", true);
            obj2.TryAdd("username", "emilesp");
            obj2.TryAdd("password", "1234567890");
            obj2.TryAdd("email", "emil@esp.com");

            jsonObject = JsonConvert.SerializeObject(obj2);
            stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var contentsec = client.PostAsync(url, stringcontent).Result;

            // Create third
            var obj3 = new ExpandoObject();
            obj3.TryAdd("isAdmin", true);
            obj3.TryAdd("username", "dánjalDJ");
            obj3.TryAdd("password", "1234567890");
            obj3.TryAdd("email", "dánjal@DJ.com");

            jsonObject = JsonConvert.SerializeObject(obj3);
            stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var contentthird = client.PostAsync(url, stringcontent).Result;

            // Read
            jsonObject = client.GetStringAsync(url).Result;
            var allusers = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);
            var newcount = allusers.Count;

            // Delete first
            HttpResponseMessage contentdel;
            var deletedone = false;
            var userone = allusers.FirstOrDefault(x => x.ElementAt(1).Value.ToString() == "basdamgaard");
            if (userone != null)
            {
                contentdel = client.DeleteAsync(url + '/' + userone.ElementAt(2).Value).Result;
                deletedone = contentdel.IsSuccessStatusCode;
            }

            // Delete second
            var deletedtwo = false;
            var usertwo = allusers.FirstOrDefault(x => x.ElementAt(1).Value.ToString() == "emilesp");
            if (usertwo != null)
            {
                contentdel = client.DeleteAsync(url + '/' + usertwo.ElementAt(2).Value).Result;
                deletedtwo = contentdel.IsSuccessStatusCode;
            }



            // Delete third
            var deletedthree = false;
            var userthree = allusers.FirstOrDefault(x => x.ElementAt(1).Value.ToString() == "dánjalDJ");
            if (userthree != null)
            {
                contentdel = client.DeleteAsync(url + '/' + userthree.ElementAt(2).Value).Result;
                deletedthree = contentdel.IsSuccessStatusCode;
            }

            //// count
            jsonObject = client.GetStringAsync(url).Result;
            var postCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            Assert.False(preCount == newcount, "Some objects not added");
            Assert.True(contentone.IsSuccessStatusCode, "Post first user failed");
            Assert.True(contentsec.IsSuccessStatusCode, "Post second user failed");
            Assert.True(contentthird.IsSuccessStatusCode, "Post third user failed");
            Assert.True(string.Equals(obj1.ElementAt(1).Value, userone.ElementAt(1).Value), "First object could not be found after being inserted");
            Assert.True(string.Equals(obj2.ElementAt(1).Value, usertwo.ElementAt(1).Value), "Second object could not be found after being inserted");
            Assert.True(string.Equals(obj3.ElementAt(1).Value, userthree.ElementAt(1).Value), "Third object could not be found after being inserted");
            Assert.True(deletedone, "Delete first user failed");
            Assert.True(deletedtwo, "Delete second user failed");
            Assert.True(deletedthree, "Delete third user failed");
            Assert.True(preCount == postCount, "Some objects not deleted");
        }

        [Fact]
        public void TestURDUser()
        {
            client = new HttpClient();
            // count
            var url = "http://dlsadminapi.azurewebsites.net/api/users/2760";
            var jsonObject = client.GetStringAsync(url).Result;
            var objectUpdate = JsonConvert.DeserializeObject<ExpandoObject>(jsonObject);

            var obj = new ExpandoObject();
            obj.TryAdd(objectUpdate.ElementAt(0).Key, true);
            obj.TryAdd(objectUpdate.ElementAt(1).Key, "sebastian");
            obj.TryAdd(objectUpdate.ElementAt(3).Key, "1234567890");
            obj.TryAdd(objectUpdate.ElementAt(4).Key, "seb@dam.com");

            // Update
            jsonObject = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = client.PutAsync(url, stringcontent).Result;

            // Read
            jsonObject = client.GetStringAsync(url).Result;
            var updatedUser = JsonConvert.DeserializeObject<ExpandoObject>(jsonObject);

            // Change back
            var obj2 = new ExpandoObject();
            obj2.TryAdd(objectUpdate.ElementAt(0).Key, objectUpdate.ElementAt(0).Value);
            obj2.TryAdd(objectUpdate.ElementAt(1).Key, objectUpdate.ElementAt(1).Value);
            obj2.TryAdd(objectUpdate.ElementAt(3).Key, objectUpdate.ElementAt(3).Value);
            obj2.TryAdd(objectUpdate.ElementAt(4).Key, objectUpdate.ElementAt(4).Value);

            jsonObject = JsonConvert.SerializeObject(obj);
            stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var contentBack = client.PutAsync(url, stringcontent).Result;

            // Read
            jsonObject = client.GetStringAsync(url).Result;
            var changedBackUser = JsonConvert.DeserializeObject<ExpandoObject>(jsonObject);

            Assert.True(content.IsSuccessStatusCode, "Fail during PUT method call");
            Assert.True(string.Equals(obj.ElementAt(1).Value, updatedUser.ElementAt(1).Value), "Element not updated");
            Assert.True(contentBack.IsSuccessStatusCode, "Failed calling PUT method to change updated value back");
            Assert.True(string.Equals(obj2.ElementAt(1).Value, changedBackUser.ElementAt(1).Value), "Element not updated back to original value");
        }

        [Fact]
        public void TestCantCreateUserExist()
        {
            client = new HttpClient(); 
            //var url = "http://dlsadminapi.azurewebsites.net/api/users";
            //var url = "https://localhost:44374/api/users";


            var obj = new ExpandoObject();
            obj.TryAdd("isAdmin", true);
            obj.TryAdd("username", "helloworld2");
            obj.TryAdd("password", "1234567890");
            obj.TryAdd("email", "seb@dam.com");

            // Create
            var jsonObject = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = client.PostAsync(url, stringcontent).Result;

            // Read
            var userjson = client.GetStringAsync(url + "/helloworld/1234567890").Result;
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(userjson);

            // Same username insert
            var obj2 = new ExpandoObject();
            obj2.TryAdd("isAdmin", true);
            obj2.TryAdd("username", "helloworld2");
            obj2.TryAdd("password", "987654321");
            obj2.TryAdd("email", "dam@seb.com");

            // Create same user
            jsonObject = JsonConvert.SerializeObject(obj);
            stringcontent = new StringContent(jsonObject);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var contentPostSame = client.PostAsync(url, stringcontent).Result;

            // Delete
            var contentdel = client.DeleteAsync(url + '/' + newObject.ElementAt(2).Value).Result;

            Assert.True(content.IsSuccessStatusCode, "Post new user failed");
            Assert.True(string.Equals(obj.ElementAt(1).Value, obj2.ElementAt(1).Value), "Objects has to have same username to test");
            Assert.False(contentPostSame.IsSuccessStatusCode, "Fail: Post existing user succeded");
            Assert.True(contentdel.IsSuccessStatusCode, "Deleted new user failed");
        }
    }
}
   
