using MovieRecommendationLibrary.Model;
using Neo4jClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendationLibrary.Neo4jDatabaseHandler
{
    public partial class Neo4jDatabaseHandler
    {
        
        //Create a user, only if they don't already exist
        public void CreateMovie(Movie newMovie)
        {
            Console.WriteLine($"CreateMovie: {newMovie.MovieId}");
            try
            {
                var list = newMovie.GenreList;
                newMovie.GenreList = null;
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();
                
                    graphClient.Cypher
                        .Create("(user:Movie {newMovie})")
                        .WithParam("newMovie", newMovie)
                        .ExecuteWithoutResults();

                    if (list?.Count > 0)
                        SetMovieGenre(newMovie.MovieId, list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Movie GetMovie(long movieId)
        {
            Movie result = null;
            
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    result = graphClient.Cypher
                        .Match($"(movie:Movie {{ movieId:  {movieId}  }})")
                        .Return(movie => movie.As<Movie>())
                        .Results
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        
        public List<Movie> GetAllMovies(bool includeGenres = false)
        {
            List<Movie> result = null;

            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    result = graphClient.Cypher
                        .Match("(movie:Movie)")
                        .Return(movie => movie.As<Movie>())
                        .Results
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void UpdateMovie(Movie updateMovie)
        {
            try
            {
                SetMovieGenre(updateMovie.MovieId, updateMovie.GenreList);
                updateMovie.GenreList = null;
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();
                    graphClient.Cypher
                        .Match("(movie:Movie)")
                        .Where((Movie movie) => movie.MovieId == updateMovie.MovieId)
                        .Set("movie = {m}")
                        .WithParam("m", updateMovie)
                        .ExecuteWithoutResults();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMovie(long movieId)
        {
            using (var graphClient = CreateClient())
            {
                graphClient.Connect();

                graphClient.Cypher
                    .Match($"(movie:Movie {{ movieId: {movieId} }})")
                    .OptionalMatch("()-[r]->(movie)")
                    .Delete("r, movie")
                    .ExecuteWithoutResults();
            }
        }

        public void DeleteAllMovies()
        {
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    graphClient.Cypher
                        .Match("(movie:Movie)")
                        .OptionalMatch("()-[r]->(movie)-[g]->()")
                        .Delete("g, r, movie")
                        .ExecuteWithoutResults();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //public dynamic DeleteAllMoviesResult()
        //{
        //    dynamic result = null;
        //    try
        //    {
        //        using (var graphClient = CreateClient())
        //        {
        //            graphClient.Connect();

        //            result = graphClient.Cypher
        //                .Match("(movie:Movie)")
        //                .OptionalMatch("()-[r]->(movie)-[g]->()")
        //                .Delete("g, r, movie")
        //                .Return(movie => movie.As<dynamic>());
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return result;
        //}


        /// <summary>
        /// Genres
        /// </summary>
        /// 


        public List<Genre> GetGenres()
        {
            List<Genre> result = null;
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    result = graphClient.Cypher
                        .Match("(genre:Genre)")
                        .Return(genre => genre.As<Genre>())
                        .Results
                        .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }


        public List<Movie> GetMoviesThatHaveGenre(string genreString)
        {
            List<Movie> result = null;
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    result = graphClient.Cypher
                        .Match($"(genre:Genre {{ name: {genreString} }})<-(movie:Movie)")
                        .Return(movie => movie.As<Movie>())
                        .Results
                        .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }


        

        public void SetMovieGenre(long movieId, List<Genre> genreList)
        {
            try
            {
                DeleteMovieGenreRelation(movieId);


                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();
                    
                    foreach (Genre currentGenre in genreList)
                    {
                        graphClient.Cypher
                            .Merge($"(genre:Genre {{ name: '{currentGenre.GenreName}' }})")
                            .With("genre")
                            .Match($"(movie:Movie {{ movieId: { movieId } }} )")
                            .CreateUnique($"(movie)-[:HAS_GENRE]->(genre)")
                            .ExecuteWithoutResults();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public Movie GetMovieWithGenres(long movieId)
        {
            Movie result = null;

            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();


                    var query = graphClient.Cypher
                        .Match("(movie:Movie)-[r]->(genre:Genre)")
                        .Where("movie.movieId = {userIdParam}")
                        .WithParams(new
                        {
                            userIdParam = movieId
                        })
                        .Return((movie, genre) => new {
                            M = movie.As<Movie>(),
                            G = genre.As<Genre>()
                        });

                    var results = query.Results.ToList();
                    var nNodes = new List<dynamic>();
                    foreach (var result1 in results)
                    {
                        if (result == null) {
                            result = result1.M;
                            result.GenreList = new List<Genre>();
                        }

                        result.GenreList.Add(result1.G);

                        //nNodes.Add(JsonConvert.DeserializeObject<dynamic>(result1.M.Data));
                        //var sdf = JsonConvert.DeserializeObject<Genre>(result1.G.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<Movie> GetAllMoviesIncludeGenres()
        {
            List<Movie> result = null;

            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();


                    var query = graphClient.Cypher
                        .Match("(movie:Movie)-[r]->(genre:Genre)")
                        //.Where("movie.movieId = {userIdParam}")
                        ////.AndWhere("b.Name = {bNameParam}")
                        //.WithParams(new
                        //{
                        //    userIdParam = 2
                        //})
                        .Return((movie, genre) => new {
                            A = movie.As<Movie>(),
                            N = genre.As<Node<string>>()
                        });

                    var results = query.Results.ToList();
                    var nNodes = new List<dynamic>();
                    foreach (var result1 in results)
                    {
                        nNodes.Add(JsonConvert.DeserializeObject<dynamic>(result1.N.Data));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public void DeleteMovieGenreRelation(long movieId)
        {
            using (var graphClient = CreateClient())
            {
                graphClient.Connect();

                graphClient.Cypher
                    .Match($"(m:Movie {{ movieId: {movieId} }})")
                    .OptionalMatch("(m)-[r]->(genre)")
                    .Delete("r")
                    .ExecuteWithoutResults();
            }
        }


        public void DeleteGenre(string name)
        {
            using (var graphClient = CreateClient())
            {
                graphClient.Connect();

                graphClient.Cypher
                    .Match($"(genre:Genre {{ name: {name} }})")
                    .OptionalMatch("()-[r]->(genre)")
                    .Delete("r, genre")
                    .ExecuteWithoutResults();
            }
        }

        public void DeleteAllGenres()
        {
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    graphClient.Cypher
                        .Match("(genre:Genre)")
                        .OptionalMatch("()-[r]->(genre)")
                        .Delete("r, genre")
                        .ExecuteWithoutResults();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
