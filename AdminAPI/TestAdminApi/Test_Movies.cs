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
    public class Test_Movies
    {
        HttpClient client;

        [Fact]
        public async void TestConnection()
        {
            client = new HttpClient();
            var check = await client.GetAsync(string.Format("http://dlsadminapi.azurewebsites.net/api/movies/{0}", 4));

            Assert.True(check.IsSuccessStatusCode);
        }

        [Fact]
        public async void TestCreateMovie()
        {


            // to delete
            int new_id = 0;
            client = new HttpClient();
            var url = "http://dlsadminapi.azurewebsites.net/api/movies/";

            try
            {

                // count

                var jsonObject = await client.GetStringAsync(url);
                var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

                var obj = new ExpandoObject();
                obj.TryAdd("title", "Hiltevarok");
                obj.TryAdd("releaseYear", 1337);


                // Create
                var jsonstring = JsonConvert.SerializeObject(obj);
                var stringcontent = new StringContent(jsonstring);
                stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var content = await client.PostAsync(url, stringcontent);

                // Read
                jsonObject = await client.GetStringAsync(url);
                var newcount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;
                var list = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);
                var movie = list.ElementAt(newcount - 1);

                var old_t = obj.ElementAt(0).Value;
                var old_y = Convert.ToInt64(obj.ElementAt(1).Value);
                new_id = Convert.ToInt32(movie.ElementAt(0).Value);
                var new_t = movie.ElementAt(1).Value;
                var new_y = movie.ElementAt(2).Value;





                // Assert.True(content.IsSuccessStatusCode);

                Assert.True(String.Equals(old_t, new_t), "Are not same");
                Assert.Equal(old_y, new_y);

            }
            catch (Exception ex)
            {
                Assert.True(false, "Exception");
            }
            finally {
                // do cleanup
                await client.DeleteAsync(url + new_id);

            }



        }
        [Fact]
        public async void TestDeleteMovie()
        {
            // to delete
            int new_id = 0;
            client = new HttpClient();
            var url = "http://dlsadminapi.azurewebsites.net/api/movies/";


            // count

            var jsonObject = await client.GetStringAsync(url);
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            var obj = new ExpandoObject();
            obj.TryAdd("title", "Hiltevarok");
            obj.TryAdd("releaseYear", 1337);


            // Create
            var jsonstring = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonstring);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = await client.PostAsync(url, stringcontent);

            // Read
            jsonObject = await client.GetStringAsync(url);
            var count = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count - 1;
            var list = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);
            var add_movie = list.ElementAt(count);

            var a_id = add_movie.ElementAt(0).Value;
            var a_title = add_movie.ElementAt(1).Value;
            var a_year = add_movie.ElementAt(2).Value;




            await client.DeleteAsync(url + new_id);

            var moviejson = await client.GetStringAsync(url + "/" + count);
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(moviejson);


            var m_id = newObject.ElementAt(0).Value;
            var m_title = newObject.ElementAt(1).Value;
            var m_year = newObject.ElementAt(2).Value;



            // Assert.True(content.IsSuccessStatusCode);

            Assert.NotEqual(a_id, m_id);




        }

        [Fact]
        public async void TestReadMovie()
        {

            client = new HttpClient();
            var url = "http://dlsadminapi.azurewebsites.net/api/movies/";

            var obj = new ExpandoObject();
            obj.TryAdd("id", 2);
            obj.TryAdd("title", "Jumanji");
            obj.TryAdd("releaseYear", 1995);

            // Read
            var moviejson = await client.GetStringAsync(url + obj.ElementAt(0).Value);
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(moviejson);




            Assert.Equal(obj.ElementAt(0).Value, Convert.ToInt32(newObject.ElementAt(0).Value));
            Assert.Equal(obj.ElementAt(1).Value, newObject.ElementAt(1).Value);
            Assert.Equal(obj.ElementAt(2).Value, Convert.ToInt32(newObject.ElementAt(2).Value));
            var movfiejson = await client.GetStringAsync(url + obj.ElementAt(0).Value);




        }

        //TODO
        [Fact]
        public async void TestUpdateMovie()
        {


            // to delete
            int new_id = 2;
            client = new HttpClient();
            var url = "http://dlsadminapi.azurewebsites.net/api/movies/";

            var old_obj = new ExpandoObject();
            old_obj.TryAdd("id", 2);
            old_obj.TryAdd("title", "Jumanji");
            old_obj.TryAdd("releaseYear", 1995);

            var o_id = old_obj.ElementAt(0).Value;
            var o_title = old_obj.ElementAt(1).Value;
            var o_year = old_obj.ElementAt(2).Value;

        

            // count
            try
            {

                var obj = new ExpandoObject();
                obj.TryAdd("id", 2);
                obj.TryAdd("title", "Hilarok");
                obj.TryAdd("releaseYear", 1337);

                // Read
                var oldMoviejson = await client.GetStringAsync(url + obj.ElementAt(0).Value);
                var oldObject = JsonConvert.DeserializeObject<ExpandoObject>(oldMoviejson);

                // PUT
                var jsonstring = JsonConvert.SerializeObject(obj);
                var stringcontent = new StringContent(jsonstring);
                stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var content = await client.PutAsync(url+o_id, stringcontent);

                // READ new obejct
                var newMoviejson = await client.GetStringAsync(url + obj.ElementAt(0).Value);
                var newObject = JsonConvert.DeserializeObject<ExpandoObject>(newMoviejson);

                var n_id = newObject.ElementAt(0).Value;
                var n_title = newObject.ElementAt(1).Value;
                var n_year = newObject.ElementAt(2).Value;

                Assert.Equal(o_id, Convert.ToInt32(n_id));
                Assert.NotEqual(o_title, n_title);
                Assert.NotEqual(o_year, Convert.ToInt32(n_year));


            }
            
            catch (Exception ex)
            {
                Assert.True(false, "Exception");
            }
            finally
            {
                // do cleanup
                var jsonstring = JsonConvert.SerializeObject(old_obj);
                var stringcontent = new StringContent(jsonstring);
                stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var content = await client.PutAsync(url, stringcontent);

            }



            }
        //TODO
        [Fact]
        public async void TestSearchForMov()
        {
            // to delete
            int new_id = 0;
            client = new HttpClient();
            var url = "http://dlsadminapi.azurewebsites.net/api/movies/";


            // count

            var jsonObject = await client.GetStringAsync(url);
            var preCount = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count;

            var obj = new ExpandoObject();
            obj.TryAdd("title", "Hiltevarok");
            obj.TryAdd("releaseYear", 1337);


            // Create
            var jsonstring = JsonConvert.SerializeObject(obj);
            var stringcontent = new StringContent(jsonstring);
            stringcontent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var content = await client.PostAsync(url, stringcontent);

            // Read
            jsonObject = await client.GetStringAsync(url);
            var count = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject).Count - 1;
            var list = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonObject);
            var add_movie = list.ElementAt(count);

            var a_id = add_movie.ElementAt(0).Value;
            var a_title = add_movie.ElementAt(1).Value;
            var a_year = add_movie.ElementAt(2).Value;




            await client.DeleteAsync(url + new_id);

            var moviejson = await client.GetStringAsync(url + "/" + count);
            var newObject = JsonConvert.DeserializeObject<ExpandoObject>(moviejson);


            var m_id = newObject.ElementAt(0).Value;
            var m_title = newObject.ElementAt(1).Value;
            var m_year = newObject.ElementAt(2).Value;



            // Assert.True(content.IsSuccessStatusCode);

            Assert.NotEqual(a_id, m_id);




        }



    }
}



