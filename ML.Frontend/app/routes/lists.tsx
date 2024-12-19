import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import axiosInstance from '../axiosInstance';

export default function MovieLists() {
  const [lists, setLists] = useState([]);
  const [newListName, setNewListName] = useState('');
  const [selectedList, setSelectedList] = useState(null);
  const [shareUser, setShareUser] = useState('');
  const [editingList, setEditingList] = useState(null);
  const [editingListName, setEditingListName] = useState('');
  const [shareModalOpen, setShareModalOpen] = useState(false);
  const [shareLink, setShareLink] = useState('');

  useEffect(() => {
    fetchLists();
  }, []);

  const fetchLists = async () => {
    const response = await axiosInstance.get('/api/MovieList');
    setLists(response.data);
  };

  const createList = async () => {
    await axiosInstance.post('/api/MovieList', {
      name: newListName,
      sharedWith: [],
      movies: []
    });
    setNewListName('');
    fetchLists();
  };

  const editList = async (id, name) => {
    await axiosInstance.put(`/api/MovieList/${id}`, { name });
    fetchLists();
  };

  const deleteList = async (id) => {
    await axiosInstance.delete(`/api/MovieList/${id}`);
    fetchLists();
  };

  const shareList = async (id) => {
    await axiosInstance.post(`/api/MovieList/${id}/share`, { username: shareUser });
    setShareUser('');
  };

  const generateShareLink = (id) => {
    const link = `${window.location.origin}/share/${id}`;
    setShareLink(link);
    setShareModalOpen(true);
  };

  const openEditModal = (list) => {
    setEditingList(list);
    setEditingListName(list.name);
  };

  const closeEditModal = () => {
    setEditingList(null);
    setEditingListName('');
  };

  const saveListName = async () => {
    if (editingList) {
      await editList(editingList.id, editingListName);
      closeEditModal();
    }
  };

  return (
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
                onClick={() => deleteList(list.id)}
                className="ml-2 px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
              >
                Delete
              </button>
            </div>
            <div className="mb-4">
              <h3 className="text-lg font-semibold text-gray-700 dark:text-gray-300">Movies:</h3>
              <ul className="list-disc list-inside">
                {list.movies && list.movies.length > 0 ? (
                  list.movies.map((movie) => (
                    <li key={movie.id} className="text-gray-600 dark:text-gray-400">{movie.title}</li>
                  ))
                ) : (
                  <li className="text-gray-600 dark:text-gray-400">No movies in this list</li>
                )}
              </ul>
            </div>
            <div className="flex items-center space-x-2">
              <button
                onClick={() => openEditModal(list)}
                className="px-4 py-2 bg-yellow-600 text-white rounded-md hover:bg-yellow-700"
              >
                Edit
              </button>
              <button
                onClick={() => generateShareLink(list.id)}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
              >
                Share
              </button>
            </div>
          </div>
        ))}
      </div>

      {editingList && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
            <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100 mb-4">Edit List Name</h3>
            <input
              type="text"
              value={editingListName}
              onChange={(e) => setEditingListName(e.target.value)}
              className="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 mb-4"
            />
            <div className="flex justify-end space-x-2">
              <button
                onClick={closeEditModal}
                className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
              >
                Cancel
              </button>
              <button
                onClick={saveListName}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
              >
                Save
              </button>
            </div>
          </div>
        </div>
      )}

      {shareModalOpen && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
            <h3 className="text-lg font-bold text-gray-800 dark:text-gray-100 mb-4">Share List</h3>
            <input
              type="text"
              value={shareUser}
              onChange={(e) => setShareUser(e.target.value)}
              placeholder="Share with user"
              className="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 mb-4"
            />
            <button
              onClick={() => shareList(selectedList.id)}
              className="w-full px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 mb-4"
            >
              Share with User
            </button>
            <div className="flex justify-between items-center">
              <span className="text-gray-600 dark:text-gray-400 break-all">{shareLink}</span>
              <button
                onClick={() => navigator.clipboard.writeText(shareLink)}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
              >
                Copy Link
              </button>
            </div>
            <div className="flex justify-end space-x-2 mt-4">
              <button
                onClick={() => setShareModalOpen(false)}
                className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
              >
                Close
              </button>
            </div>
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