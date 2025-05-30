import { Card, CardContent } from "../ui/card";
import { Button } from "../ui/button";
import { Book, X } from "lucide-react";
import { BookingStatus } from "@/interface/interface";

export default function HistoryBookingItem({ booking, onCancel }) {
	const formatDate = (isoDate: string) => {
		const date = new Date(isoDate);
		return date.toLocaleDateString("fr-FR");
	};

	const getStatusLabel = (status: BookingStatus) => {
		switch (status) {
			case BookingStatus.BOOKED:
				return { label: "Réservée", color: "bg-green-100 text-green-800" };
			case BookingStatus.CANCELLED:
				return { label: "Annulée", color: "bg-red-100 text-red-800" };
			case BookingStatus.COMPLETED:
				return { label: "Terminée", color: "bg-gray-200 text-gray-800" };
			case BookingStatus.NOSHOW:
				return { label: "Non honorée", color: "bg-yellow-100 text-yellow-800" };
			default:
				return { label: "Inconnue", color: "bg-gray-100 text-gray-700" };
		}
	};

	const status = getStatusLabel(booking.status);

	const today = new Date();
	today.setHours(0, 0, 0, 0);

	const bookingDate = new Date(booking.date);

	const canBeCancelled =
		booking.status === BookingStatus.BOOKED && bookingDate >= today;

	const handleCancel = () => {
		const confirm = window.confirm(
			"Voulez-vous vraiment annuler cette réservation ?"
		);
		if (confirm && onCancel) {
			onCancel(booking.id); // Appelle la fonction parent
		}
	};

	return (
		<Card key={booking.id} className="shadow-md">
			<CardContent className="p-4">
				<div className="flex items-center justify-between">
					<div className="flex items-center gap-2">
						<Book className="text-gray-600" size={20} />
						<span className="font-semibold text-lg">
							Emplacement {booking.slotId}
						</span>
					</div>
					<span
						className={`px-2 py-1 text-xs rounded-full font-medium ${status.color}`}
					>
						{status.label}
					</span>
				</div>
				<p className="text-sm text-gray-500 mt-2">
					Date : {formatDate(booking.date)}
				</p>

				{canBeCancelled && (
					<div className="mt-4">
						<Button
							variant="destructive"
							onClick={handleCancel}
							className="flex items-center gap-1"
						>
							<X size={16} /> Annuler
						</Button>
					</div>
				)}
			</CardContent>
		</Card>
	);
}
