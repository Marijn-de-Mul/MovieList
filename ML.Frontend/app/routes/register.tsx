import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import Cookies from 'js-cookie';

export default function Register() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post('/api/Auth/register', { username, password });
      const response = await axiosInstance.post('/api/Auth/login', { username, password });
      Cookies.set('auth-token', response.data.token);
      navigate('/');
    } catch (error) {
      alert('Registration failed');
    }
  };

  return (
    <div className="flex h-screen items-center justify-center bg-gray-100 dark:bg-gray-900 px-4">
      <div className="w-full max-w-sm p-6 space-y-8 bg-white dark:bg-gray-800 rounded-lg shadow-lg">
        <div className="flex flex-col items-center gap-4">
          <h2 className="text-3xl font-bold text-gray-800 dark:text-gray-100">Register</h2>
          <div className="h-20 w-20">
            <img
              src="/logo-light.png"
              alt="MovieList"
              className="block w-full dark:hidden"
            />
            <img
              src="/logo-dark.png"
              alt="MovieList"
              className="hidden w-full dark:block"
            />
          </div>
        </div>
        <form className="mt-8 space-y-6" onSubmit={handleRegister}>
          <div className="rounded-md shadow-sm space-y-4">
            <div>
              <label htmlFor="username" className="sr-only">Username</label>
              <input
                id="username"
                name="username"
                type="text"
                autoComplete="username"
                required
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 dark:text-gray-100 dark:placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                placeholder="Username"
              />
            </div>
            <div>
              <label htmlFor="password" className="sr-only">Password</label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="new-password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 dark:text-gray-100 dark:placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                placeholder="Password"
              />
            </div>
          </div>
          <div>
            <button
              type="submit"
              className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              Register
            </button>
          </div>
        </form>
        <div className="text-sm text-center">
          <p className="text-gray-600 dark:text-gray-400">
            Already have an account?{' '}
            <a href="/login" className="font-medium text-blue-600 hover:text-blue-500 dark:text-blue-400 dark:hover:text-blue-300">
              Login
            </a>
          </p>
        </div>
      </div>
    </div>
  );
}