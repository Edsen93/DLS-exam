
CREATE OR REPLACE FUNCTION  search_movie(
movie_name text)
RETURNS TABLE (
	movie_id int,
	movie_title text,
    movie_year NUMERIC
	) 
AS $$
DECLARE
count_results int;
BEGIN		
	SELECT  count(*)
	INTO count_results
	FROM movies m
	WHERE m.title ILIKE '%' || movie_name || '%';
	
	IF count_results IS NOT NULL AND count_results > 10 THEN
		RETURN QUERY
		SELECT  m.id, m.title, m.releaseyear
		FROM movies m
		WHERE m.title ILIKE '%' || movie_name || '%';
	ELSE
	RETURN QUERY
		SELECT  m.id, m.title,  m.releaseyear
		FROM movies m
		WHERE m.title ILIKE '%' || movie_name || '%' OR m.title % movie_name OR metaphone(m.title, 20)=metaphone(movie_name, 20);
	END IF;
END;
$$ LANGUAGE plpgsql; 
