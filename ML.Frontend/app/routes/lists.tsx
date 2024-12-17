import React, { useState, useEffect } from 'react';
import axios from 'axios';

export default function MovieLists() {
  const [lists, setLists] = useState([]);
  const [newListName, setNewListName] = useState('');
  const [selectedList, setSelectedList] = useState(null);
  const [shareUser, setShareUser] = useState('');

  useEffect(() => {
    fetchLists();
  }, []);

  const fetchLists = async () => {
    const response = await axios.get('/api/MovieList');
    setLists(response.data);
  };

  const createList = async () => {
    await axios.post('/api/MovieList', { name: newListName });
    setNewListName('');
    fetchLists();
  };

  const editList = async (id, name) => {
    await axios.put(`/api/MovieList/${id}`, { name });
    fetchLists();
  };

  const deleteList = async (id) => {
    await axios.delete(`/api/MovieList/${id}`);
    fetchLists();
  };

  const shareList = async (id) => {
    await axios.post(`/api/MovieList/${id}/share`, { username: shareUser });
    setShareUser('');
  };

  const removeUserFromList = async (id, userId) => {
    await axios.delete(`/api/MovieList/${id}/user/${userId}`);
  };

  return (
    <div className="flex flex-col items-center p-4 bg-gray-100 dark:bg-gray-900 min-h-screen">
      <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mb-8">Movie Lists</h1>
      <div className="w-full max-w-2xl space-y-4">
        <div className="flex items-center space-x-2">
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
          <div key={list.id} className="p-4 bg-white dark:bg-gray-800 rounded-lg shadow-md">
            <div className="flex justify-between items-center">
              <input
                type="text"
                value={selectedList === list.id ? list.name : ''}
                onChange={(e) => editList(list.id, e.target.value)}
                onFocus={() => setSelectedList(list.id)}
                onBlur={() => setSelectedList(null)}
                className="flex-grow px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <button
                onClick={() => deleteList(list.id)}
                className="ml-2 px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
              >
                Delete
              </button>
            </div>
            <div className="mt-4 flex items-center space-x-2">
              <input
                type="text"
                value={shareUser}
                onChange={(e) => setShareUser(e.target.value)}
                placeholder="Share with user"
                className="flex-grow px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <button
                onClick={() => shareList(list.id)}
                className="px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
              >
                Share
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}