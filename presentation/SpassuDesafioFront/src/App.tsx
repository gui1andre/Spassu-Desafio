import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import { PrivateRoute, PublicRoute } from './components/PrivateRoute';
import { Navbar } from './components/Navbar';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { Pedidos } from './pages/Pedidos';
import { NovoPedido } from './pages/NovoPedido';
import { PedidoDetalhe } from './pages/PedidoDetalhe';
import './App.css';

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route
              path="/login"
              element={
                <PublicRoute>
                  <Login />
                </PublicRoute>
              }
            />
            <Route
              path="/register"
              element={
                <PublicRoute>
                  <Register />
                </PublicRoute>
              }
            />
            <Route
              path="/pedidos"
              element={
                <PrivateRoute>
                  <Pedidos />
                </PrivateRoute>
              }
            />
            <Route
              path="/pedidos/novo"
              element={
                <PrivateRoute>
                  <NovoPedido />
                </PrivateRoute>
              }
            />
            <Route
              path="/pedidos/:id"
              element={
                <PrivateRoute>
                  <PedidoDetalhe />
                </PrivateRoute>
              }
            />
            <Route path="*" element={<Navigate to="/pedidos" replace />} />
          </Routes>
        </main>
      </AuthProvider>
    </BrowserRouter>
  );
}
