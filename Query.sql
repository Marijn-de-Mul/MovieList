-- Create "MovieLists" table
CREATE TABLE "MovieLists" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "userId" INTEGER NOT NULL
);

-- Create "Movies" table
CREATE TABLE "Movies" (
    "Id" SERIAL PRIMARY KEY,
    "TheMovieDbId" INTEGER NOT NULL,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL
);

-- Create "Users" table
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" TEXT NOT NULL,
    "Password" TEXT NOT NULL
);

-- Create "MovieDTO" table
CREATE TABLE "MovieDTO" (
    "Id" INTEGER PRIMARY KEY,
    "TheMovieDbId" INTEGER NOT NULL,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    CONSTRAINT "FK_MovieDTO_MovieLists" FOREIGN KEY ("Id") REFERENCES "MovieLists"("Id") ON DELETE CASCADE
);

-- Create "UserDTO" table
CREATE TABLE "UserDTO" (
    "Id" INTEGER PRIMARY KEY,
    "Username" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    CONSTRAINT "FK_UserDTO_MovieLists" FOREIGN KEY ("Id") REFERENCES "MovieLists"("Id") ON DELETE CASCADE
);

-- Create "MovieListMovies" table
CREATE TABLE "MovieListMovies" (
    "MovieListId" INTEGER NOT NULL,
    "MovieId" INTEGER NOT NULL,
    CONSTRAINT "PK_MovieListMovies" PRIMARY KEY ("MovieListId", "MovieId"),
    CONSTRAINT "FK_MovieListMovies_MovieLists" FOREIGN KEY ("MovieListId") REFERENCES "MovieLists"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MovieListMovies_Movies" FOREIGN KEY ("MovieId") REFERENCES "Movies"("Id") ON DELETE CASCADE
);

-- Create "MovieListSharedWith" table
CREATE TABLE "MovieListSharedWith" (
    "MovieListId" INTEGER NOT NULL,
    "UserId" INTEGER NOT NULL,
    CONSTRAINT "PK_MovieListSharedWith" PRIMARY KEY ("MovieListId", "UserId"),
    CONSTRAINT "FK_MovieListSharedWith_MovieLists" FOREIGN KEY ("MovieListId") REFERENCES "MovieLists"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_MovieListSharedWith_Users" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
