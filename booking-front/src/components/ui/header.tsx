import { useNavigate } from "react-router-dom";
import { Menu } from "lucide-react";
import {
	DropdownMenu,
	DropdownMenuTrigger,
	DropdownMenuContent,
	DropdownMenuItem,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";

export default function Header() {
	const navigate = useNavigate();

	const handleLogout = () => {
		localStorage.removeItem("authToken"); // ou selon ton système d'auth
		navigate("/login");
	};

	return (
		<header className="flex items-center justify-between px-6 py-4 bg-black text-white shadow-md">
			{/* Titre de l'app */}
			<button
				onClick={() => navigate("/booking")}
				className="text-xl font-bold cursor-pointer"
			>
				MonApp Parking
			</button>

			{/* Menu burger */}
			<DropdownMenu>
				<DropdownMenuTrigger asChild>
					<Button variant="ghost" size="icon" className="text-white">
						<Menu className="h-6 w-6" />
					</Button>
				</DropdownMenuTrigger>
				<DropdownMenuContent align="end" className="bg-white text-black">
					<DropdownMenuItem onClick={() => navigate("/history")}>
						Historique des réservations
					</DropdownMenuItem>
					<DropdownMenuItem onClick={handleLogout}>
						Déconnexion
					</DropdownMenuItem>
				</DropdownMenuContent>
			</DropdownMenu>
		</header>
	);
}
