// pages/UnauthorizedPage.tsx
export default function UnauthorizedPage() {
	return (
		<div className="text-center mt-20">
			<h1 className="text-3xl font-bold text-red-500">⛔ Accès refusé</h1>
			<p className="mt-2 text-gray-600">
				Vous n’avez pas la permission d’accéder à cette page.
			</p>
		</div>
	);
}
