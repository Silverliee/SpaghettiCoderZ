import AuthService from "@/services/authService";
import { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
	const [user, setUser] = useState(() => {
		// Chargement initial depuis localStorage
		const stored = localStorage.getItem("user");
		return stored ? JSON.parse(stored) : null;
	});

	useEffect(() => {
		if (user) {
			localStorage.setItem("user", JSON.stringify(user));
		} else {
			localStorage.removeItem("user");
		}
	}, [user]);

	const login = async (data) => {
		const loginData = await AuthService.login(data);
		setUser(loginData); // { userId, token, ... }
	};

	const logout = async () => {
		await AuthService.logout();
		setUser(null);
	};

	return (
		<AuthContext.Provider value={{ user, login, logout }}>
			{children}
		</AuthContext.Provider>
	);
}

export function useAuth() {
	return useContext(AuthContext);
}
