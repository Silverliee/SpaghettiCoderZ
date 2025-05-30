// layouts/WithHeaderLayout.tsx
import Header from "@/components/ui/header";
import { Outlet } from "react-router-dom";

export default function WithHeaderLayout() {
	return (
		<>
			<Header />
			<main className="p-4">
				<Outlet />
			</main>
		</>
	);
}
