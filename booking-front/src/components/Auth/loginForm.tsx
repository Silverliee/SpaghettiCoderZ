import { useState } from "react";
import { login } from "../../services/authService";
import { useNavigate } from "react-router-dom";

import { Mail, Lock } from "lucide-react";

export default function LoginForm() {
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState<string | null>(null);
	const navigate = useNavigate();

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		setError(null);

		try {
			const res = await login({ email, password });
			localStorage.setItem("token", res.token);
			navigate("/dashboard");
		} catch (err) {
			console.error(err);
			setError("Identifiants invalides");
		}
	};

	return (
		<form
			onSubmit={handleSubmit}
			className="max-w-md mx-auto p-8 bg-white rounded-lg shadow-md"
		>
			<h2 className="text-3xl font-semibold mb-6 text-center">Connexion</h2>

			<label className="relative block mb-6">
				<Mail
					className="absolute top-1/2 left-3 -translate-y-1/2 text-gray-400"
					size={20}
				/>
				<input
					type="email"
					value={email}
					onChange={(e) => setEmail(e.target.value)}
					placeholder="Email"
					required
					className="w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
				/>
			</label>

			<label className="relative block mb-6">
				<Lock
					className="absolute top-1/2 left-3 -translate-y-1/2 text-gray-400"
					size={20}
				/>
				<input
					type="password"
					value={password}
					onChange={(e) => setPassword(e.target.value)}
					placeholder="Mot de passe"
					required
					className="w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
				/>
			</label>

			{error && (
				<p className="text-sm text-red-600 mb-4 text-center font-medium">
					{error}
				</p>
			)}

			<button
				type="submit"
				className="w-full py-3 bg-blue-600 hover:bg-blue-700 transition-colors text-white font-semibold rounded-md"
			>
				Se connecter
			</button>
		</form>
	);
}
