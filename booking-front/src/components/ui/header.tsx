import { useNavigate } from "react-router-dom";
import { Menu } from "lucide-react";
import {
	DropdownMenu,
	DropdownMenuTrigger,
	DropdownMenuContent,
	DropdownMenuItem,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/contexts/AuthContext";
import { UserRole } from "@/interface/interface";

export default function Header() {
	const navigate = useNavigate();

	const handleLogout = () => {
		localStorage.removeItem("user");
		navigate("/");
	};

	const { user } = useAuth();
	const userRole = user?.role;

	return (
		<header className="relative flex items-center justify-center px-6 py-4 bg-black text-white shadow-md">
			<h1
				onClick={() => navigate("/booking")}
				className="absolute left-1/2 transform -translate-x-1/2 text-xl font-bold cursor-pointer"
			>
				Parking Steward
			</h1>

			<div className="ml-auto">
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
						{userRole === UserRole.MANAGER && (
							<DropdownMenuItem onClick={() => navigate("/statistics")}>
								Statistiques
							</DropdownMenuItem>
						)}
						<DropdownMenuItem onClick={handleLogout}>
							Déconnexion
						</DropdownMenuItem>
					</DropdownMenuContent>
				</DropdownMenu>
			</div>
		</header>
	);
}
