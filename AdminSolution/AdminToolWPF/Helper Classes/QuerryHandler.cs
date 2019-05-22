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
    }
}
