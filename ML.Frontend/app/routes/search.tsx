import React, { useState } from 'react';
import axiosInstance from '../axiosInstance';

export default function Search() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  const searchMovies = async () => {
    const response = await axiosInstance.get(`/api/Movie/search?query=${query}`);
    setResults(response.data);
  };

  return (
    <div className="flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen">
      <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-8">Search Movies</h1>
      <div className="w-full max-w-2xl space-y-4">
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
        {results.length > 0 && (
          <div className="mt-4 space-y-4">
            {results.map((movie) => (
              <div key={movie.id} className="p-4 bg-white dark:bg-gray-800 rounded-lg shadow-md">
                <h2 className="text-xl font-bold text-gray-800 dark:text-gray-100">{movie.title}</h2>
                <p className="text-gray-600 dark:text-gray-400">{movie.description}</p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}