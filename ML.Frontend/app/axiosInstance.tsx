import axios from 'axios';
import Cookies from 'js-cookie';

const baseURL =
  process.env.NODE_ENV === 'production'
    ? 'http://movielist_backend:8079' 
    : 'http://localhost:8079'; 

const axiosInstance = axios.create({
  baseURL,
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = Cookies.get('auth-token');
    if (token && config.url && !config.url.includes('/api/Auth/login') && !config.url.includes('/api/Auth/register')) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axiosInstance;