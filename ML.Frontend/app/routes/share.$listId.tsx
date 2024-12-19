import React, { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../axiosInstance';
import ProtectedRoute from './components/ProtectedRoute';

export default function Share() {
    const { listId } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        const addUserToList = async () => {
            try {
                const response = await axiosInstance.get('/api/Auth/me');
                const user = response.data;

                console.log('Adding user to list:', user);
                console.log('List ID:', listId);

                await axiosInstance.post(`/api/MovieList/${listId}/share`, { id: '0', username: user.username, password: 'defaultpassword' });
                navigate(`/lists`);
            } catch (error) {
                console.error('Error adding user to list:', error);
                alert('Failed to add user to list. Please try again.');
            }
        };

        addUserToList();
    }, [listId, navigate]);

    return (
        <ProtectedRoute>
            <div className="flex items-center justify-center min-h-screen">
                <p className="text-gray-600 dark:text-gray-400 text-xl font-bold animate-pulse">
                    Adding you to the list<span className="dot1">.</span><span className="dot2">.</span><span className="dot3">.</span>
                </p>
            </div>
        </ProtectedRoute>
    );
}