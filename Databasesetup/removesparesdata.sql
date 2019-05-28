DELETE FROM movies
WHERE title is NULL;

UPDATE movies
SET releaseYear = 0
WHERE releaseYear is NULL;