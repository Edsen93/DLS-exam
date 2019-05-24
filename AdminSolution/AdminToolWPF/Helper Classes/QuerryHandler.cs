using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminToolWPF.Model;
using Newtonsoft.Json;

namespace AdminToolWPF.Helper_Classes
{
    public class QuerryHandler
    {
        public static List<Movie> GetMovies()
        {
            string CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movie";
            
            string json = RequestHandler.Get(CurrentSentQuerry);
            
            List<Movie> result = JsonConvert.DeserializeObject<List<Movie>>(json);
            
            return result;
        }


        public static Movie GetMovie(int id)
        {
            string CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movie/full/{id}";

            string json = RequestHandler.Get(CurrentSentQuerry);

            Movie result = JsonConvert.DeserializeObject<Movie>(json);

            return result;
        }



        public static bool UpdateMovie(Movie movie)
        {

            string CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movie/{movie.MovieId}";

            // public string Post(string uri, string data, string contentType, string method = "POST")



            string serialized = JsonConvert.SerializeObject(movie);

            //using (var client = new System.Net.WebClient())
            //{
            //    client.UploadData(CurrentSentQuerry, "PUT", serialized);
            //}


            var sdf = RequestHandler.Post(CurrentSentQuerry, serialized,"Movie","PUT");
            

            return true;
        }
    }
}
