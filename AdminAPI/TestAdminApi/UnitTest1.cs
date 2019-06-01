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
        public async void TestCreateUser()
        {
            client = new HttpClient();
            // count
            var url = "http://dlsadminapi.azurewebsites.net/api/users/";
            var jsonObject = await client.GetStringAsync(url);
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            var obj = new ExpandoObject();
            obj.TryAdd("isAdmin", true);
            obj.TryAdd("username", "basdamgaard");
            obj.TryAdd("password", "1234567890");
            obj.TryAdd("email", "seb@dam.com");

            // Create
            var jsonstring = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonstring);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = await client.PostAsync(url, stringcontent);

            // Read
            jsonObject = await client.GetStringAsync(url);
            var newcount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;
            var userjson = await client.GetStringAsync(url + "/basdamgaard/1234567890");
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(userjson);

            // Delete
            var listprop = newObject.ToList();
            var contentdel = await client.DeleteAsync(url + '/' + listprop[2].Value);

            //// count
            jsonObject = await client.GetStringAsync(url);
            var postCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;


            Assert.NotEqual(preCount, newcount);
            Assert.True(content.IsSuccessStatusCode);
            Assert.Equal(newObject.ElementAt(1), obj.ElementAt(1));
            Assert.Equal(preCount, postCount);
        }
    }
}
