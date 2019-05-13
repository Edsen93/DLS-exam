using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendationLibrary
{
    public class QuerryRepository
    {
        private static string GetMovieRecommendationForUserQuerry(int userId, int limit = 5)
        {
            string querry = @"
                
                MATCH (u:User {userId: " + userId + @"})-[r:RATED]->(m:Movie)
                WITH u, avg(r.rating) AS mean
                MATCH (u)-[r:RATED]->(m:Movie)-[:HAS_GENRE]->(g:Genre)
                WHERE r.rating > mean
                WITH u, g, COUNT(*) AS sscore
                MATCH (g)<-[:HAS_GENRE]-(rec:Movie)
                WHERE NOT EXISTS((u)-[:RATED]->(rec))
                RETURN rec.title as recommendation, COLLECT(DISTINCT g.name) as genres, SUM(sscore) as score
                ORDER BY score DESC LIMIT " + limit;

            return querry;
        }

        private static string GetMovieRecommendationForMovieQuerry(string movieTitle, int limit = 5)
        {
            string querry = @"
                
                MATCH (m:Movie {title: """ + movieTitle + @"""})-[:HAS_GENRE]-(t:Genre)<-[:HAS_GENRE]-(other:Movie) 
                where other.averageRating is not null
                WITH m, other, COUNT(t) AS intersection, COLLECT(t.name) AS i
                MATCH(m) -[:HAS_GENRE] - (mt)
                WITH other, intersection,i, COLLECT(mt.name) AS genreList1
                MATCH(other) -[:HAS_GENRE] - (ot)
                WITH other,intersection,i, genreList1, COLLECT(ot.name) AS genreList2
                WITH other, intersection, genreList1, genreList2, ((1.0 * intersection) / SIZE(genreList1 + filter(x IN genreList2 WHERE NOT x IN genreList1))) AS jaccard
                where jaccard > 0.7 RETURN other.title as recommendedMovie, other.averageRating as averageRating, jaccard
                ORDER BY averageRating DESC LIMIT " + limit;

            return querry;
        }

        private static string FindMovieQuerry(string searchText)
        {
            string querry = @"
                
                with 'toi~ stori~' as query
                call apoc.index.search('"+ searchText + @"', query) YIELD node as search, weight
                as score
                with query, search, score , search.title as title limit 10
                call apoc.text.phoneticDelta( title , query) YIELD delta as psim
                with title, score , psim, score *( psim/2 +1) as comb
                return title , weight , psim,comb order by comb desc

            ";

            return querry;
        }

        private static string SetMovieAverage()
        {
            string querry =  @"
                MATCH (movie:Movie)-[r:RATED]-(g:User)
                WITH movie, avg(r.rating) AS avgRating
                set movie.averageRating = avgRating
            ";

            return querry;
        }

        private static string CreateDatabaseQuerry()
        {
            string querry = @"

            //Script 1.
            create index on :Movie(movieId);
            create index on :Links(movieId);
            create index on :GenomeTag(tagId);
            create index on :User(userId);
            create index on :Genre(name);


            //Script  2.
            // Movies
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/movies.csv"" as row 
            create(movie: Movie{ movieId: toInteger(row.movieId), title: row.title});

            //Script  3.
            // Genres
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/movies.csv"" as row
            MATCH(movie: Movie { movieId: toInteger(row.movieId)})
            FOREACH(g IN split(row.genres, ""|"") | merge(ge: Genre{ name: g}));

            //Script  4.
            // Genre relation
            USING PERIODIC COMMIT 5000
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/movies.csv"" as row
            MATCH(m: Movie { movieId: toInteger(row.movieId)})
            UNWIND split(row.genres,""|"") as genreText
            with genreText, m
            MATCH(g: Genre { name: genreText})
            CREATE(m) -[:HAS_GENRE]->(g);


            //Script 5.
            // Links
            USING PERIODIC COMMIT 5000
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/links.csv"" as row
            MATCH(movie: Movie { movieId: toInteger(row.movieId)})
            CREATE(ref:Links{ movieId: toInteger(row.movieId), imdbId: toInteger(row.imdbId), tmdbId: toInteger(row.tmdbId)})
            CREATE(movie) -[:EXTERNAL_REFRENCES]->(ref);


            //Script 6.
            //GenomeTag
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/genome-tags.csv"" as row
            create(:GenomeTag{ tagId: toInteger(row.tagId), tag: row.tag});


            //Script  7.
            //Genome score
            USING PERIODIC COMMIT 5000
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/genome-scores.csv"" as row
            MATCH(genomeTag: GenomeTag { tagId: toInteger(row.tagId)}),
            (movie: Movie { movieId: toInteger(row.movieId)})
            CREATE(movie) -[:GENOME_SCORE { relevance: toFloat(row.relevance) }]->(genomeTag);


            //Script  8.
            // Rating and Users
            USING PERIODIC COMMIT 5000
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/ratings.csv"" as row
            match(movie: Movie { movieId: toInteger(row.movieId)})
            merge(user: User { userId: toInteger(row.userId)})
            CREATE(user) -[:RATED { rating: toFloat(row.rating), timestamp: row.timestamp }]->(movie);


            //Script  9.
            //Create Tag
            USING PERIODIC COMMIT 5000
            LOAD CSV with headers from ""file:///C:/Users/SharkForceOne/Downloads/ml-latest/ml-latest/tags.csv"" as row
            MATCH(movie: Movie { movieId: toInteger(row.movieId)}),
            (user: User { userId: toInteger(row.userId)})
            CREATE(user) -[:TAGGED { tag: row.tag, timestamp: row.timestamp }]->(movie);

            //Script  10.
            //Set movie average
            MATCH(movie: Movie) -[r: RATED] - (g: User)
            WITH movie, avg(r.rating) AS avgRating
            set movie.averageRating = avgRating

            // Script 11 create lucene indexes
            call apoc.index.addAllNodes('movie-title',{ Movie:['title']})

            ";
            querry += SetMovieAverage();
            return querry;
        }


    }
}
