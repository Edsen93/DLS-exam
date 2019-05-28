using MovieRecommendationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;

namespace MovieRecommendationLibrary.Neo4jDatabaseHandler
{
    public partial class Neo4jDatabaseHandler
    {
        public List<MovieRecommendation> GetRecommendationOnMovie(long movieId)
        {
            List<MovieRecommendation> result = null;
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();

                    result = graphClient.Cypher
                        .Match($"(m:Movie {{ movieId: {movieId} }})-[:HAS_GENRE]-(t:Genre)<-[:HAS_GENRE]-(other:Movie)")
                        .With("m, other, COUNT(t) AS intersection, COLLECT(t.name) AS i")
                        .Match("(m)-[:HAS_GENRE]-(mt)")
                        .With("other, intersection,i, COLLECT(mt.name) AS genreList1")
                        .Match("(other)-[:HAS_GENRE]-(ot)")
                        .With("other,intersection,i, genreList1, COLLECT(ot.name) AS genreList2")
                        .With("other,intersection, genreList1, genreList2, ((1.0*intersection)/SIZE(genreList1+filter(x IN genreList2 WHERE NOT x IN genreList1))) AS jaccard")
                        .Where("jaccard > 0.7")
                        .With(@"{   title: other.title, 
                                    movieId: other.movieId,
                                    averageRating: other.averageRating,
                                    jaccard: jaccard 
                                } as movie")
                        .Return<MovieRecommendation>(movie => movie.As<MovieRecommendation>())
                        .OrderByDescending("movie.averageRating")
                        .Limit(10)
                        .Results.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }



        public List<MovieRecommendation> GetRecommendationOnUser(long userId)
        {
            List<MovieRecommendation> result = null;
            try
            {
                using (var graphClient = CreateClient())
                {
                    graphClient.Connect();


                    //var asdfresult = graphClient.Cypher
                    //    .Match($"(u:User {{ userId: {userId} }})-[r:RATED]->(m:Movie)")
                    //    .With("u, avg(r.rating) AS mean")
                    //    .Match("(u)-[r:RATED]->(m:Movie)-[:HAS_GENRE]->(g:Genre)")
                    //    .Where("r.rating > mean")
                    //    .With("u, g, COUNT(*) AS sscore")
                    //    .Match("(g)<-[:HAS_GENRE]-(rec:Movie)")
                    //    .Where("NOT (((u)-[:RATED]-(rec)))")
                    //    .With(@"{   title: rec.title, 
                    //                movieId: rec.movieId,
                    //                averageRating: rec.averageRating,
                    //                jaccard: SUM(sscore) 
                    //            } as movie")
                    //    .Return<MovieRecommendation>(movie => movie.As<MovieRecommendation>())
                    //    .OrderByDescending("movie.averageRating")
                    //    .Limit(10);




                    //var asdresult = graphClient.Cypher
                    //    .Match($"(u:User {{ userId: {userId} }})-[r:RATED]->(m:Movie)")
                    //    .With("u, avg(r.rating) AS mean")
                    //    .Match("(u)-[r:RATED]->(m:Movie)-[:HAS_GENRE]->(g:Genre)")
                    //    .Where("r.rating > mean")
                    //    .With("u, g, COUNT(*) AS sscore")
                    //    .Match("(g)<-[:HAS_GENRE]-(rec:Movie)")
                    //    .Where("NOT (((u)-[:RATED]-(rec)))")
                    //    .With(@"{   title: rec.title, 
                    //                movieId: rec.movieId,
                    //                averageRating: rec.averageRating,
                    //                jaccard: SUM(sscore) ,
                    //                genre: COLLECT(DISTINCT g.name)
                    //            } as movie,  collect(DISTINCT g) as collection")


                    //    .Return((movie, collection) => new {
                    //        User = movie.As<MovieRecommendation>(),
                    //        glist = collection.CollectAs<Genre>()
                    //    })


                    //    //.Return<MovieRecommendation>(movie => movie.As<MovieRecommendation>())
                    //    .OrderByDescending("movie.averageRating")
                    //    .Limit(10)
                    //    ;//.Results.ToList();






                    userId = 5;
                    result = graphClient.Cypher
                        .Match($"(u:User {{ userId: {userId} }})-[r:RATED]->(m:Movie)")
                        .With("u, avg(r.rating) AS mean")
                        .Match("(u)-[r:RATED]->(m:Movie)-[:HAS_GENRE]->(g:Genre)")
                        .Where("r.rating > mean")
                        .With("u, g, COUNT(*) AS sscore")
                        .Match("(g)<-[:HAS_GENRE]-(rec:Movie)")
                        .Where("NOT (((u)-[:RATED]-(rec)))")
                        .AndWhere("rec.averageRating is not null")
                        .With(@"{   title: rec.title, 
                                    movieId: rec.movieId,
                                    averageRating: rec.averageRating,
                                    jaccard: SUM(sscore),
                                    genre: COLLECT(DISTINCT g.name)
                                } as movie")
                        .Return<MovieRecommendation>(movie => movie.As<MovieRecommendation>())
                        .OrderByDescending("movie.jaccard")
                        .Limit(10)
                        .Results.ToList();




                    //MATCH(u: User { userId: 5})-[r: RATED]->(m: Movie)
                    //WITH u, avg(r.rating) AS mean
                    //MATCH(u) -[r: RATED]->(m: Movie) -[:HAS_GENRE]->(g: Genre)
                    //WHERE r.rating > mean
                    //WITH u, g, COUNT(*) AS sscore
                    //MATCH(g) < -[:HAS_GENRE] - (rec: Movie)
                    //WHERE NOT EXISTS((u) -[:RATED]->(rec))
                    //RETURN rec.title as recommendation, COLLECT(DISTINCT g.name) as genres, SUM(sscore) as score
                    //ORDER BY score DESC LIMIT 3



                    //                    RETURN rec.title as recommendation, COLLECT(DISTINCT g.name) as genres, SUM(sscore) as score
                    //                    ORDER BY score DESC LIMIT 3

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}
