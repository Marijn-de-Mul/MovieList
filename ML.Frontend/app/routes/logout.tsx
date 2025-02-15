import React, { useEffect } from 'react';
import { useNavigate } from '@remix-run/react';
import Cookies from 'js-cookie';

export default function Logout() {
  const navigate = useNavigate();

  useEffect(() => {
    Cookies.remove('auth-token');
    setTimeout(() => {
      window.location.href = '/login';
    }, 100); 
  }, [navigate]);

  return (
    <div className="flex h-screen items-center justify-center bg-gray-100 dark:bg-gray-900">
      <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100">Logging out...</h1>
    </div>
  );
}