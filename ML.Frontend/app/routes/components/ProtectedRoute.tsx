import { useEffect } from 'react';
import { useNavigate } from '@remix-run/react';
import Cookies from 'js-cookie';

const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const navigate = useNavigate();
  const isAuthenticated = !!Cookies.get('auth-token');

  useEffect(() => {
    if (!isAuthenticated) {
      navigate(`/login?redirect=${window.location.pathname}`);
    }
  }, [isAuthenticated, navigate]);

  return isAuthenticated ? <>{children}</> : null;
};

export default ProtectedRoute;