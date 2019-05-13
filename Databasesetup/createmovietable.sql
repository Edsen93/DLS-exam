DROP TABLE IF EXISTS  movies;

CREATE TABLE movies
(
    id SERIAL PRIMARY KEY,
    title  TEXT ,
    releaseYear NUMERIC 
);
SET CLIENT_ENCODING TO 'UTF8';
COPY movies (title, releaseYear) 
FROM 'C:\Users\edsen\Documents\db scripts\moviesPostgres.csv' DELIMITER ',' CSV HEADER;


DELETE FROM movies
WHERE title is NULL;

UPDATE movies
SET releaseYear = 0
WHERE releaseYear is NULL;
