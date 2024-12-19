import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import axiosInstance from '../axiosInstance';

export default function Search() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [lists, setLists] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  // Function to search movies based on the query
  const searchMovies = async () => {
    setIsLoading(true);
    try {
      const response = await axiosInstance.get(`/api/Movie/search?query=${query}`);
      setResults(response.data);
      console.log('Search results:', response.data);
    } catch (error) {
      console.error('Error searching movies:', error);
    }
    setIsLoading(false);
  };

  // Function to fetch all movie lists
  const fetchLists = async () => {
    try {
      const response = await axiosInstance.get('/api/MovieList');
      setLists(response.data);
      console.log('Fetched lists:', response.data);
    } catch (error) {
      console.error('Error fetching lists:', error);
    }
  };

  // Function to add a movie to a specific list
  const addMovieToList = async (movieId, listId) => {
    console.log(`addMovieToList called with Movie ID: ${movieId}, List ID: ${listId}`);

    // Ensure movieId and listId are strings for consistent comparison
    const movie = results.find((movie) => String(movie.id) === String(movieId));
    if (!movie) {
      console.error('Movie not found:', movieId);
      return;
    }

    const list = lists.find((list) => String(list.id) === String(listId));
    if (!list) {
      console.error('List not found:', listId);
      return;
    }

    // Check if the movie is already in the list to prevent duplicates
    const isAlreadyAdded = list.movies.some((m) => String(m.id) === String(movieId));
    if (isAlreadyAdded) {
      alert('Movie is already in the selected list!');
      return;
    }

    const updatedMovies = [...list.movies, movie];
    console.log('Updated movies for the list:', updatedMovies);

    try {
      await axiosInstance.put(`/api/MovieList/${listId}`, { name: list.name, movies: updatedMovies });
      alert('Movie added to list!');
      setIsModalOpen(false);
    } catch (error) {
      console.error('Error adding movie to list:', error);
      alert('Failed to add movie to list. Please try again.');
    }
  };

  // Function to open the modal and select a movie
  const openModal = async (movieId) => {
    console.log(`Opening modal for Movie ID: ${movieId}`);
    setSelectedMovie(movieId);
    await fetchLists();
    setIsModalOpen(true);
  };

  // Function to close the modal
  const closeModal = () => {
    console.log('Closing modal');
    setIsModalOpen(false);
    setSelectedMovie(null);
  };

  return (
    <div className="relative flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen">
      <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-8">Search Movies</h1>
      
      {/* Search Input and Button */}
      <div className={`w-full max-w-2xl space-y-4 ${isLoading ? 'opacity-50' : 'opacity-100'} transition-opacity duration-300`}>
        <div className="flex items-center space-x-2">
          <input
            type="text"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search for a movie"
            className="flex-grow px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <button
            onClick={searchMovies}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            Search
          </button>
        </div>

        {/* Display Search Results */}
        {!isLoading && results.length > 0 && (
          <div className="mt-4 space-y-4">
            {results.map((movie) => (
              <div key={movie.id} className="p-4 bg-white dark:bg-gray-800 rounded-lg shadow-md">
                <h2 className="text-xl font-bold text-gray-800 dark:text-gray-100">{movie.title}</h2>
                <p className="text-gray-600 dark:text-gray-400">{movie.description}</p>
                <button
                  onClick={() => {
                    console.log('Add to List clicked for movie:', movie.id);
                    openModal(movie.id);
                  }}
                  className="mt-2 px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
                >
                  Add to List
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Loading Indicator */}
      {isLoading && (
        <div className="absolute inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="flex flex-col items-center">
            <p className="text-gray-600 dark:text-gray-400 text-xl font-bold animate-pulse">
              Searching<span className="dot1">.</span><span className="dot2">.</span><span className="dot3">.</span>
            </p>
          </div>
        </div>
      )}

      {/* Modal to Select List */}
      {isModalOpen && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
            <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100 mb-4">Select List</h3>
            <div className="space-y-4">
              {lists.length > 0 ? (
                lists.map((list) => (
                  <div key={list.id} className="flex items-center justify-between">
                    <span className="text-gray-800 dark:text-gray-100">{list.name}</span>
                    <button
                      onClick={() => {
                        console.log('Add button clicked for list:', list.id);
                        addMovieToList(selectedMovie, list.id);
                      }}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                    >
                      Add
                    </button>
                  </div>
                ))
              ) : (
                <p className="text-gray-600 dark:text-gray-400">No lists available. Please create a list first.</p>
              )}
            </div>
            <button
              onClick={closeModal}
              className="mt-4 px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
            >
              Close
            </button>
          </div>
        </div>
      )}

      {/* Back to Main Menu Button */}
      <Link
        to="/"
        className="fixed bottom-4 right-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
      >
        Back to Main Menu
      </Link>
    </div>
  );
}