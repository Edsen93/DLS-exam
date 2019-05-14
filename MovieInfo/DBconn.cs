using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ms.MovieInfo.Models;
using Npgsql;


namespace ms.MovieInfo
{
    // guide https://github.com/dalegambill/PostgreSql_and_Csharp/blob/master/PostgreSQL/PostGreSQL.cs
    public class MoviePersistence
    {
        Movie dummyMovie = new Movie();
        private NpgsqlConnection conn;

        public MoviePersistence()
        {
            String myConnectionString;

            string server = "127.0.0.1";
            string database = "MovieInfo";
            string user = "admin";
            string password = "admin";
         
            myConnectionString = String.Format("Server=movieinfo.postgres.database.azure.com;Database=movies;Port=5432;User Id=dlsgroup2019@movieinfo;Password=Recomender2019;Ssl Mode=Require;");
            


            try {


                conn = new NpgsqlConnection(myConnectionString);
                conn.Open();
                
                
            }
            catch (NpgsqlException ex)
            {

            }

        }
        public Movie SaveMovie(Movie movieToSave)
         {
            string sqlString =String.Format("INSERT INTO movies(title, releaseYear) VALUES ('{0}',{1}) RETURNING id, title,releaseYear ", movieToSave.title.ToString(), movieToSave.releaseYear);

            NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn);
            
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            Movie m = new Movie();

                while(dataReader.Read())
            {
                m.ID = dataReader.GetInt32(0);
                m.title= dataReader.GetString(1);
                m.releaseYear = dataReader.GetInt32(2);

            }
 
            return m;

        }
        
        public void deleteMovie(int id)
            //DELETE FROM movies WHERE id = {0};

        {
            string sqlString = String.Format("DELETE FROM movies WHERE id = {0}", id);

            NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
          


        }
        // Search on title, handle input error, will always return a list.
        public List<Movie> SearchForMovie()

        {
            List<Movie> movies = new List<Movie>();
            string sqlString = "Select * From movies";

            NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {

                Movie m = new Movie();
                m.ID = dataReader.GetInt32(0);
                m.title = dataReader.GetString(1);
                m.releaseYear = dataReader.GetInt32(2);

                
                movies.Add(m);
            }
                conn.Close();
            return movies;


        }
        //
        public Movie findOneMovie(int id)
        {

            

            string sqlString =String.Format("Select * From movies WHERE id ={0}", id);      
            Movie m = new Movie();
          

            NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                m.ID = dataReader.GetInt32(0);
                m.title = dataReader.GetString(1);
                m.releaseYear = dataReader.GetInt32(2);
            }

           
            return m;


        }
   
        // Update movie bases on ID.
        public Movie updateMovie(int id, Movie movieToDelete)

        {
            string sqlString = String.Format("UPDATE movies SET (title = '{0}', releaseYear = {1} WHERE id = {2}) RETURNING id, title,releaseYear", movieToDelete.title, movieToDelete.releaseYear, id);
            NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn);
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            cmd.ExecuteNonQuery();
            Movie m = new Movie();

            while (dataReader.Read())
            {
                m.ID = dataReader.GetInt32(0);
                m.title = dataReader.GetString(1);
                m.releaseYear = dataReader.GetInt32(2);

            }

            return m;

        }
    }
}

