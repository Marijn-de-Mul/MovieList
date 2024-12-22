import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import Cookies from 'js-cookie';
import ProtectedRoute from './components/ProtectedRoute';

const EditList = () => {
  const { listId } = useParams();
  const navigate = useNavigate();
  const [listName, setListName] = useState('');
  const [movies, setMovies] = useState([]);
  const [sharedWith, setSharedWith] = useState([]);
  const [loading, setLoading] = useState(true);

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
      setListName(selectedList.name);
      setMovies(selectedList.movies);
      setSharedWith(selectedList.sharedWith.map((sw) => sw.user));
      setLoading(false);
    } catch (error) {
      console.error('Error fetching lists:', error);
    }
  };

  const handleSave = async () => {
    try {
      await axiosInstance.post('', {
        endpoint: `/api/MovieList/${listId}`,
        method: 'PUT',
        authorization: Cookies.get('auth-token'),
        body: { name: listName, movies, sharedWith },
        contentType: 'application/json',
      });
      navigate(`/list-details/${listId}`);
    } catch (error) {
      console.error('Error saving list:', error);
    }
  };

  const handleDelete = async () => {
    try {
      await axiosInstance.post('', {
        endpoint: `/api/MovieList/${listId}`,
        method: 'DELETE',
        authorization: Cookies.get('auth-token'),
        body: null,
        contentType: 'application/json',
      });
      navigate('/lists');
    } catch (error) {
      console.error('Error deleting list:', error);
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-100 dark:bg-gray-900 flex items-center justify-center p-4">
        <div className="w-full max-w-4xl space-y-8">
          <h1 className="text-4xl font-bold text-gray-800 dark:text-gray-100 text-center mb-8">Edit List</h1>
          <div>
            <label className="block text-gray-700 dark:text-gray-300 mb-2">List Name</label>
            <input
              type="text"
              value={listName}
              onChange={(e) => setListName(e.target.value)}
              className="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div>
            <h3 className="text-2xl font-semibold text-gray-700 dark:text-gray-300 mb-4">Shared With</h3>
            <ul className="list-disc list-inside space-y-2">
              {sharedWith.length > 0 ? (
                sharedWith.map((sw) => (
                  <li key={sw.id} className="text-gray-600 dark:text-gray-400">
                    {sw.username}
                  </li>
                ))
              ) : (
                <li className="text-gray-600 dark:text-gray-400">No users shared with this list</li>
              )}
            </ul>
          </div>
          <div className="flex justify-center space-x-4 mt-8">
            <button
              onClick={handleSave}
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              Save
            </button>
            <button
              onClick={() => navigate(`/list-details/${listId}`)}
              className="px-4 py-2 bg-gray-600 text-white rounded-md hover:bg-gray-700"
            >
              Cancel
            </button>
            <button
              onClick={handleDelete}
              className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default EditList;