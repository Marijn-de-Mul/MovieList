import React, { useState, useEffect } from 'react';
import { useParams, Link } from '@remix-run/react';
import axiosInstance from '../axiosInstance';
import ProtectedRoute from './components/ProtectedRoute';

export default function MovieDetails() {
  const { movieId } = useParams();
  const [movie, setMovie] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        const response = await axiosInstance.get(`/api/Movie/details/${movieId}`);
        setMovie(response.data);
        console.log('Fetched movie details:', response.data);
      } catch (error) {
        console.error('Error fetching movie details:', error);
      }
      setIsLoading(false);
    };

    fetchMovieDetails();
  }, [movieId]);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <p className="text-gray-600 dark:text-gray-400 text-xl font-bold animate-pulse">
          Loading<span className="dot1">.</span><span className="dot2">.</span><span className="dot3">.</span>
        </p>
      </div>
    );
  }

  if (!movie) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <p className="text-gray-600 dark:text-gray-400 text-xl font-bold">
          Movie not found
        </p>
      </div>
    );
  }

  return (
    <ProtectedRoute>
      <div className="relative flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen overflow-hidden">
        <div className="w-full max-w-md flex flex-col items-center">
          <img
            src="https://via.placeholder.com/200x300"
            alt="Movie Cover"
            className="w-auto max-w-xs h-[30vh] object-cover rounded-lg shadow-md mb-8"
          />
          <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-4 text-center">{movie.title}</h1>
          <p className="text-gray-600 dark:text-gray-400 mb-8 text-center">{movie.description}</p>
        </div>
        <Link
          to="/"
          className="fixed bottom-4 right-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
        >
          Back to Main Menu
        </Link>
      </div>
    </ProtectedRoute>
  );
}