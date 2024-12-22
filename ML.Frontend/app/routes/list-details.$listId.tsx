import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import Cookies from 'js-cookie';
import ProtectedRoute from './components/ProtectedRoute';

const ListDetails = () => {
  const { listId } = useParams();
  const navigate = useNavigate();
  const [list, setList] = useState(null);

  useEffect(() => {
    if (listId) {
      fetchLists();
    }
  }, [listId]);

  const fetchLists = async () => {
    try {
      const response = await axiosInstance.post('', {
        endpoint: '/api/MovieList',
        method: 'GET',
        authorization: Cookies.get('auth-token'),
        body: null,
        contentType: 'application/json',
      });
      const lists = response.data;
      const selectedList = lists.find((list) => list.id === parseInt(listId));
      setList(selectedList);
    } catch (error) {
      console.error('Error fetching lists:', error);
    }
  };

  const viewMovieDetails = (movieId) => {
    navigate(`/movie/${movieId}`);
  };

  const deleteMovieFromList = async (movieId) => {
    try {
      await axiosInstance.post('', {
        endpoint: `/api/MovieList/${listId}/movies/${movieId}`,
        method: 'DELETE',
        authorization: Cookies.get('auth-token'),
        body: null,
        contentType: 'application/json',
      });
      fetchLists();
    } catch (error) {
      console.error('Error deleting movie from list:', error);
    }
  };

  const editList = () => {
    navigate(`/edit-list/${listId}`);
  };

  const shareList = () => {
    navigate(`/share-list/${listId}`);
  };

  if (!list) {
    return <div>Loading...</div>;
  }

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-100 dark:bg-gray-900 flex items-center justify-center p-4">
        <div className="w-full max-w-4xl space-y-8">
          <h1 className="text-4xl font-bold text-gray-800 dark:text-gray-100 text-center mb-8">{list.name}</h1>
          <div>
            <h3 className="text-2xl font-semibold text-gray-700 dark:text-gray-300 mb-4">Quick Stats</h3>
            <ul className="list-disc list-inside space-y-2">
              <li className="text-gray-600 dark:text-gray-400">Movies: {list.movies.length}</li>
              <li className="text-gray-600 dark:text-gray-400">Shared with: {list.sharedWith.length} users</li>
            </ul>
          </div>
          <hr className="border-gray-300 dark:border-gray-700" />
          <div>
            <h3 className="text-2xl font-semibold text-gray-700 dark:text-gray-300 mb-4">Movies</h3>
            <ul className="list-disc list-inside space-y-2">
              {list.movies && list.movies.length > 0 ? (
                list.movies.map((movieItem) => (
                  <li
                    key={movieItem.movie.id}
                    className="text-gray-600 dark:text-gray-400 cursor-pointer flex justify-between items-center"
                  >
                    <span onClick={() => viewMovieDetails(movieItem.movie.id)}>{movieItem.movie.title}</span>
                    <button
                      onClick={() => deleteMovieFromList(movieItem.movie.id)}
                      className="ml-2 px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
                    >
                      Delete
                    </button>
                  </li>
                ))
              ) : (
                <li className="text-gray-600 dark:text-gray-400">No movies in this list</li>
              )}
            </ul>
          </div>
          <hr className="border-gray-300 dark:border-gray-700" />
          <div>
            <h3 className="text-2xl font-semibold text-gray-700 dark:text-gray-300 mb-4">Shared With</h3>
            <ul className="list-disc list-inside space-y-2">
              {list.sharedWith && list.sharedWith.length > 0 ? (
                list.sharedWith.map((sw) => (
                  <li key={sw.user.id} className="text-gray-600 dark:text-gray-400">
                    {sw.user.username}
                  </li>
                ))
              ) : (
                <li className="text-gray-600 dark:text-gray-400">No users shared with this list</li>
              )}
            </ul>
          </div>
          <div className="flex justify-center space-x-4 mt-8">
            <button
              onClick={editList}
              className="px-4 py-2 bg-yellow-600 text-white rounded-md hover:bg-yellow-700"
            >
              Edit
            </button>
            <button
              onClick={shareList}
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              Share
            </button>
            <button
              onClick={() => navigate('/lists')}
              className="px-4 py-2 bg-gray-600 text-white rounded-md hover:bg-gray-700"
            >
              Back to Lists
            </button>
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default ListDetails;