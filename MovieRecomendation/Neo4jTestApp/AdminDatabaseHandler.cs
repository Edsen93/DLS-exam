using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Neo4jClient;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using CypherNet.Transaction;
using CypherNet.Configuration;
using System.IO;
using System.Windows;
using MovieRecommendationLibrary;
using MovieRecommendationLibrary.Model;
using MovieRecommendationLibrary.Neo4jDatabaseHandler;

namespace Neo4jTestApp
{

    //public class CountResult
    //{
    //    [JsonProperty(PropertyName = "count")]
    //    public String Count { get; set; }
    //    [JsonProperty(PropertyName = "topId")]
    //    public String TopId { get; set; }
    //}

    //public class User
    //{
    //    [JsonProperty(PropertyName = "userId")]
    //    public long UserId { get; set; }
    //}

    //public class UserRating
    //{
    //    public long UserId { get; set; }
    //    public string MovieId { get; set; }
    //    public string Rating { get; set; }
    //    public string Timestamp { get; set; }
    //}

    //public class Movie
    //{
    //    [JsonProperty(PropertyName = "title")]
    //    public string Title { get; set; }

    //    [JsonProperty(PropertyName = "movieId")]
    //    public int MovieId { get; set; }

    //    [JsonProperty(PropertyName = "averageRating")]
    //    public float? AverageRating { get; set; }

    //    [JsonProperty(PropertyName = "jaccard")]
    //    public float? Jaccard { get; set; }
    //}


    public class AdminDatabaseHandler
    {
        

        //get current ammount of users

        // ask how menny users do you want

        // validate the input

        // load users



        public static long? CurrentNumberCount()
        {
            long? result = null;

            try
            {
                var client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "password");
                client.Connect();


                var querryResult = client.Cypher
                   .Match("(u:User)")
                   .Return<User>(u => u.As<User>())
                   .OrderByDescending("u.userId")
                   .Limit(1)
                   .Results;

                result = querryResult.LastOrDefault()?.UserId;
            }
            catch (Exception)
            {
            }
            
            

            return result;
        }

        public static string CreateDatabasesFromDataset(bool neo4j, bool postgres, int? userCount )
        {
            string start = $"StartTime: {DateTime.Now.ToString("HH:mm:sszzz")}";
            Console.WriteLine(start);
            CreateMoviesFromDataset(neo4j, postgres);
            string mid = $"End movie: {DateTime.Now.ToString("HH:mm:sszzz")}";
            Console.WriteLine(mid);

            if (!userCount.HasValue)
                CreateUsersFromDataset(userCount.Value, neo4j, postgres);

            string end = $"EndUser: {DateTime.Now.ToString("HH:mm:sszzz")}";
            Console.WriteLine(end);


            return start + Environment.NewLine + mid + Environment.NewLine + end;
        }


        // Create Movies
        private static void CreateMoviesFromDataset(bool neo4j, bool postgres)
        {
            Neo4jDatabaseHandler neoHandler = new Neo4jDatabaseHandler();
            neoHandler.DeleteAllGenres();
            neoHandler.DeleteAllMovies();

            long? currentCount = CurrentNumberCount();

            if (!currentCount.HasValue)
                currentCount = 0;

            // Clear User Relations 
            // Then clear users

            string fileLocation = AppContext.BaseDirectory + "Files\\movies.csv";

            using (TextFieldParser parser = new TextFieldParser(fileLocation))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                bool first = true;
                

                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    //TODO: Process field
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        string title = fields[1];
                        string[] splitLine = title.Split('(', ')');

                        string year = "";

                        if (splitLine.Length > 2)
                            year = splitLine[splitLine.Length - 2];

                        bool parsed = Int32.TryParse(year, out int x);
                        if (!parsed)
                            year = "";
                        
                        title = splitLine[0];
                        if (string.IsNullOrWhiteSpace(title))
                            title = splitLine[1];
                        

                        bool contains = title.Contains(",");
                        if (contains)
                        {
                            string[] splitTitle = title.Split(',');

                            string partOne = splitTitle[1];
                            if (partOne.Length < 2)
                                partOne = "";
                            else
                                partOne = splitTitle[1].Substring(1, splitTitle[1].Length - 2);


                            string partTwo = splitTitle[0];
                            title = partOne + " " + partTwo;
                        }

                        title = title.Substring(0, title.Length - 1);
                        
                        int recid = int.Parse(fields[0]);

                        bool hasYear = int.TryParse(year, out int releaseYear);
                        Movie movie = new Movie()
                        {
                            MovieId = recid,
                            Title = title
                        };

                        if (hasYear)
                            movie.ReleaseYear = releaseYear;


                        //List<Genre> genre = 


                        string genreFullString = fields[2];

                        List<string> ss = genreFullString.Split('|').ToList();
                        List<Genre> gen = new List<Genre>();
                        foreach (var item in ss)
                        {
                            gen.Add(new Genre() { GenreName = item });
                        }

                        neoHandler.CreateMovie(movie);
                        neoHandler.SetMovieGenre(movie.MovieId, gen);
                    }
                }
            }
        }



        // Create Users
        private static void CreateUsersFromDataset(int userCount, bool neo4j, bool postgres)
        {
            long? currentCount = CurrentNumberCount();

            if (!currentCount.HasValue)
                currentCount = 0;

            // Clear User Relations 
            // Then clear users
            
            string fileLocation = AppContext.BaseDirectory + "Files\\ratings.csv";

            using (TextFieldParser parser = new TextFieldParser(fileLocation))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                bool first = true;
                bool run = true;

                List<UserRating> userReviewList = new List<UserRating>();
                
                while (!parser.EndOfData && run)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    //TODO: Process field
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        long userId = long.Parse(fields[0]);


                        //Checks if it still is the same user, if not then the current will be sent to the db
                        if (userReviewList.Count != 0 && userReviewList[0].UserId != userId)
                        {
                            CreateUserReviews(userReviewList);
                            
                            //Clear the list, so that the data is not duplicated
                            userReviewList.Clear();
                        }



                        if (currentCount.Value >= userId)
                        {

                        }
                        else if (userId > userCount)
                        {
                            run = false;
                            break;
                        }
                        else
                        {
                            long movieId = long.Parse(fields[1]);
                            int rating = int.Parse(fields[2]);

                            userReviewList.Add(
                                new UserRating
                                {
                                    UserId = userId,
                                    MovieId = movieId,
                                    Rating = rating,
                                    Timestamp = fields[3]
                                }
                            );
                        }
                    }

                }

            }
        }

        private static bool CreateUserReviews(List<UserRating> list)
        {

            ////Create user;
            //var session = new CypherSession(new ConnectionProperties(BaseUri), mock.Object);
            //var node = session.CreateNode(new { name = Name }, "person");

            long userId = list.First().UserId;

            try
            {
                Neo4jDatabaseHandler handler = new Neo4jDatabaseHandler();

                handler.CreateUser(new User {  UserId = userId });

                foreach (var item in list)
                {
                    handler.SetUserMovieRating(userId, item.MovieId, item.Rating);
                }    
            }
            catch (Exception ex)
            {
            }

            return true;
        }
    }
}
