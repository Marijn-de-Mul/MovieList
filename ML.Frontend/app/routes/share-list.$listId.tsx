import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import Cookies from 'js-cookie';
import ProtectedRoute from './components/ProtectedRoute';

const ShareList = () => {
  const { listId } = useParams();
  const navigate = useNavigate();
  const [shareUser, setShareUser] = useState('');
  const [matchingUsers, setMatchingUsers] = useState([]);
  const [shareLink, setShareLink] = useState('');
  const [sharedWith, setSharedWith] = useState([]);

  useEffect(() => {
    if (listId) {
      generateShareLink();
      fetchSharedUsers();
    }
  }, [listId]);

  const generateShareLink = () => {
    const link = `${window.location.origin}/login?redirect=/share/${listId}`;
    setShareLink(link);
  };

  const fetchSharedUsers = async () => {
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
      setSharedWith(selectedList.sharedWith.map((sw) => sw.user));
    } catch (error) {
      console.error('Error fetching shared users:', error);
    }
  };

  const searchUsers = async (query) => {
    if (query.length > 0) {
      try {
        const response = await axiosInstance.post('', {
          endpoint: `/api/Auth/search?query=${query}`,
          method: 'GET',
          authorization: Cookies.get('auth-token'),
          body: null,
          contentType: 'application/json',
        });
        setMatchingUsers(response.data);
      } catch (error) {
        console.error('Error searching users:', error);
      }
    } else {
      setMatchingUsers([]);
    }
  };

  const shareList = async (user) => {
    try {
      await axiosInstance.post('', {
        endpoint: `/api/MovieList/${listId}/share`,
        method: 'POST',
        authorization: Cookies.get('auth-token'),
        body: user,
        contentType: 'application/json',
      });
      setShareUser('');
      setMatchingUsers([]);
      fetchSharedUsers();
      alert('User added to the list!');
    } catch (error) {
      console.error('Error sharing list:', error);
      alert('Failed to share list. Please try again.');
    }
  };

  const removeSharedUser = async (userId) => {
    try {
      await axiosInstance.post('', {
        endpoint: `/api/MovieList/${listId}/user/${userId}`,
        method: 'DELETE',
        authorization: Cookies.get('auth-token'),
        body: null,
        contentType: 'application/json',
      });
      fetchSharedUsers();
      alert('User removed from the list!');
    } catch (error) {
      console.error('Error removing shared user:', error);
      alert('Failed to remove user. Please try again.');
    }
  };

  return (
    <ProtectedRoute>
      <div className="min-h-screen bg-gray-100 dark:bg-gray-900 flex items-center justify-center p-4">
        <div className="w-full max-w-4xl space-y-8">
          <h1 className="text-4xl font-bold text-gray-800 dark:text-gray-100 text-center mb-8">Share List</h1>
          <div>
            <label className="block text-gray-700 dark:text-gray-300 mb-2">Share with User</label>
            <input
              type="text"
              value={shareUser}
              onChange={(e) => {
                setShareUser(e.target.value);
                searchUsers(e.target.value);
              }}
              className="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            {matchingUsers.length > 0 && (
              <ul className="w-full bg-white dark:bg-gray-800 border rounded-md mb-4 max-h-40 overflow-auto">
                {matchingUsers.map((user) => (
                  <li
                    key={user.id}
                    className="px-4 py-2 cursor-pointer hover:bg-gray-100 dark:hover:bg-gray-700"
                    onClick={() => {
                      setShareUser(user.username);
                      setMatchingUsers([]);
                    }}
                  >
                    {user.username}
                  </li>
                ))}
              </ul>
            )}
            <button
              onClick={() => shareList({ id: 0, username: shareUser, password: '' })}
              className="mt-[3vh] w-full px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 mb-4"
            >
              Share with User
            </button>
            <div className="flex justify-between items-center mb-4">
              <span className="text-gray-600 dark:text-gray-400 break-all">{shareLink}</span>
              <button
                onClick={() => navigator.clipboard.writeText(shareLink)}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
              >
                Copy Link
              </button>
            </div>
          </div>
          <div>
            <h3 className="text-2xl font-semibold text-gray-700 dark:text-gray-300 mb-4">Shared With</h3>
            <ul className="list-disc list-inside space-y-2">
              {sharedWith.length > 0 ? (
                sharedWith.map((user) => (
                  <li
                    key={user.id}
                    className="text-gray-600 dark:text-gray-400 flex justify-between items-center"
                  >
                    <span>{user.username}</span>
                    <button
                      onClick={() => removeSharedUser(user.id)}
                      className="ml-2 px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
                    >
                      Remove
                    </button>
                  </li>
                ))
              ) : (
                <li className="text-gray-600 dark:text-gray-400">No users shared with this list</li>
              )}
            </ul>
          </div>
          <div className="flex justify-center space-x-4 mt-8">
            <button
              onClick={() => navigate(`/list-details/${listId}`)}
              className="px-4 py-2 bg-gray-600 text-white rounded-md hover:bg-gray-700"
            >
              Back to List
            </button>
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default ShareList;