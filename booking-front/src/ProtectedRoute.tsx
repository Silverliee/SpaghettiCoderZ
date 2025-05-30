// components/auth/ProtectedRoute.tsx
import { Navigate, Outlet } from "react-router-dom";
import { UserRole } from "@/interface/interface"; // Ton enum

interface ProtectedRouteProps {
	allowedRoles: UserRole[];
}

export default function ProtectedRoute({ allowedRoles }: ProtectedRouteProps) {
	const storedUser = localStorage.getItem("user");
	const user = storedUser ? JSON.parse(storedUser) : null;

	if (!user) {
		return <Navigate to="/" replace />;
	}

	if (!allowedRoles.includes(user.role)) {
		// Rediriger ou afficher un message d'accès refusé
		return <Navigate to="/unauthorized" replace />;
	}

	return <Outlet />;
}
