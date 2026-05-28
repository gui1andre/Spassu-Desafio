import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <Link to="/pedidos" className="navbar-brand">
        Sistema de Pedidos
      </Link>
      {user && (
        <div className="navbar-right">
          <span className="navbar-user">{user.email}</span>
          <button onClick={handleLogout} className="btn-outline btn-sm">
            Sair
          </button>
        </div>
      )}
    </nav>
  );
}
