CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR NOT NULL,
    "Password" VARCHAR NOT NULL
);

CREATE TABLE "Movies" (
    "Id" SERIAL PRIMARY KEY,
    "TheMovieDbId" INT NOT NULL,
    "Title" VARCHAR NOT NULL,
    "Description" VARCHAR NOT NULL
);

CREATE TABLE "MovieLists" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR NOT NULL,
    "userId" INT NOT NULL,
    FOREIGN KEY ("userId") REFERENCES "Users"("Id")
);

CREATE TABLE "MovieListMovies" (
    "MovieListId" INT NOT NULL,
    "MovieId" INT NOT NULL,
    PRIMARY KEY ("MovieListId", "MovieId"),
    FOREIGN KEY ("MovieListId") REFERENCES "MovieLists"("Id"),
    FOREIGN KEY ("MovieId") REFERENCES "Movies"("Id")
);

CREATE TABLE "MovieListSharedWith" (
    "MovieListId" INT NOT NULL,
    "UserId" INT NOT NULL,
    PRIMARY KEY ("MovieListId", "UserId"),
    FOREIGN KEY ("MovieListId") REFERENCES "MovieLists"("Id"),
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id")
);

CREATE TABLE "MovieDTO" (
    "Id" SERIAL PRIMARY KEY,
    "Title" VARCHAR NOT NULL,
    "Description" VARCHAR NOT NULL
);

CREATE TABLE "UserDTO" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR NOT NULL,
    "Password" VARCHAR NOT NULL
);