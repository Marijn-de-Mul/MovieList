import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import ProtectedRoute from './components/ProtectedRoute';
import Cookies from 'js-cookie';

export default function MovieLists() {
  const [lists, setLists] = useState([]);
  const [newListName, setNewListName] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchLists();
  }, []);

  const fetchLists = async () => {
    try {
      const response = await axiosInstance.post('', {
        endpoint: '/api/MovieList',
        method: 'GET',
        authorization: Cookies.get('auth-token'),
        body: null,
        contentType: 'application/json',
      });
      setLists(response.data);
    } catch (error) {
      console.error('Error fetching lists:', error);
    }
  };

  const createList = async () => {
    try {
      await axiosInstance.post('', {
        endpoint: '/api/MovieList',
        method: 'POST',
        authorization: Cookies.get('auth-token'),
        body: {
          name: newListName,
          sharedWith: [],
          movies: []
        },
        contentType: 'application/json',
      });
      setNewListName('');
      fetchLists();
    } catch (error) {
      console.error('Error creating list:', error);
    }
  };

  const openListDetails = (id) => {
    navigate(`/list-details/${id}`);
  };

  return (
    <ProtectedRoute>
      <div className="relative flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen overflow-hidden">
        <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-8">Movie Lists</h1>
        <div className="w-full max-w-2xl space-y-4 overflow-auto">
          <div className="flex items-center space-x-2 mb-4">
            <input
              type="text"
              value={newListName}
              onChange={(e) => setNewListName(e.target.value)}
              placeholder="New list name"
              className="flex-grow px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <button
              onClick={createList}
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              Create
            </button>
          </div>
          {lists.map((list) => (
            <div key={list.id} className="p-4 bg-white dark:bg-gray-800 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300">
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold text-gray-800 dark:text-gray-100">{list.name}</h2>
                <button
                  onClick={() => openListDetails(list.id)}
                  className="ml-2 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
                  View Details
                </button>
              </div>
              <div className="mb-4">
                <h4 className="text-lg font-semibold text-gray-700 dark:text-gray-300">Stats</h4>
                <ul className="list-disc list-inside">
                  <li className="text-gray-600 dark:text-gray-400">Movies: {list.movies.length}</li>
                  <li className="text-gray-600 dark:text-gray-400">Shared with: {list.sharedWith.length} users</li>
                </ul>
              </div>
            </div>
          ))}
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