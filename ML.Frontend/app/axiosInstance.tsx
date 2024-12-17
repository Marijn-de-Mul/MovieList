import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7196', 
});

export default axiosInstance;