// layouts/WithoutHeaderLayout.tsx
import { Outlet } from "react-router-dom";

export default function WithoutHeaderLayout() {
	return (
		<main className="p-4">
			<Outlet />
		</main>
	);
}
