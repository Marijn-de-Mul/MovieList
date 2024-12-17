import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import axiosInstance from '../axiosInstance';

export default function Search() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [listId, setListId] = useState('');
  const [lists, setLists] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const searchMovies = async () => {
    setIsLoading(true);
    const response = await axiosInstance.get(`/api/Movie/search?query=${query}`);
    setResults(response.data);
    setIsLoading(false);
  };

  const fetchLists = async () => {
    const response = await axiosInstance.get('/api/MovieList');
    setLists(response.data);
  };

  const addMovieToList = async (movieId, listId) => {
    await axiosInstance.post(`/api/MovieList/${listId}/movies`, { movieId });
    alert('Movie added to list!');
    setIsModalOpen(false);
  };

  const openModal = (movieId) => {
    setSelectedMovie(movieId);
    fetchLists();
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedMovie(null);
    setListId('');
  };

  return (
    <div className="relative flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen">
      <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-8">Search Movies</h1>
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
        {!isLoading && results.length > 0 && (
          <div className="mt-4 space-y-4">
            {results.map((movie) => (
              <div key={movie.id} className="p-4 bg-white dark:bg-gray-800 rounded-lg shadow-md">
                <h2 className="text-xl font-bold text-gray-800 dark:text-gray-100">{movie.title}</h2>
                <p className="text-gray-600 dark:text-gray-400">{movie.description}</p>
                <button
                  onClick={() => openModal(movie.id)}
                  className="mt-2 px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
                >
                  Add to List
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {isLoading && (
        <div className="absolute inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="flex flex-col items-center">
            <p className="text-gray-600 dark:text-gray-400 text-xl font-bold animate-pulse">Searching<span className="dot1">.</span><span className="dot2">.</span><span className="dot3">.</span></p>
          </div>
        </div>
      )}

      {isModalOpen && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
            <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100 mb-4">Select List</h3>
            <div className="space-y-4">
              {lists.map((list) => (
                <div key={list.id} className="flex items-center justify-between">
                  <span className="text-gray-800 dark:text-gray-100">{list.name}</span>
                  <button
                    onClick={() => addMovieToList(selectedMovie, list.id)}
                    className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                  >
                    Add
                  </button>
                </div>
              ))}
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

      <Link
        to="/"
        className="fixed bottom-4 right-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
      >
        Back to Main Menu
      </Link>
    </div>
  );
}