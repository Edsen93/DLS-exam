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
    public class RecommendationRatingTest
    {
        HttpClient client;

        [Fact]
        public void TestConnection()
        {
            client = new HttpClient();
            var check = client.GetAsync(string.Format("https://dlsadminapi.azurewebsites.net/api/movies/{0}", 4)).Result;

            Assert.True(check.IsSuccessStatusCode, "Service is not running");
        }

        [Fact]
        public void TestRecommendation()
        {
            client = new HttpClient();
            var check = client.GetAsync(string.Format("https://dlsadminapi.azurewebsites.net/api/recommendation/{0}", 4)).Result;

            Assert.True(check.IsSuccessStatusCode, "Recommendation is not working");
        }

        [Fact]
        public void TestRecommendationWithWrongId()
        {
            client = new HttpClient();
            //var url = string.Format("https://localhost:44374/api/recommendation/{0}", -9999);
            var url = string.Format("https://dlsadminapi.azurewebsites.net/api/recommendation/{0}", -9999);

            var check = client.GetAsync(url).Result;

            Assert.False(check.IsSuccessStatusCode, "Found recommendation it shouldn't");
        }

        [Fact]
        public void TestRecommendationWithStringId()
        {
            client = new HttpClient();
            var check = client.GetAsync(string.Format("https://dlsadminapi.azurewebsites.net/api/recommendation/{0}", "helloworld")).Result;

            Assert.False(check.IsSuccessStatusCode, "Found recommendation it shouldn't");
        }
    }
}
