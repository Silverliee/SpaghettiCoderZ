import { useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import { Book } from "lucide-react";
import BookingService from "@/services/bookingService";
import { BookingStatus } from "@/interface/interface";
import { Card, CardContent } from "../ui/card";
import HistoryBookingItem from "./history-booking-item";

export default function HistoryDisplay() {
	const [bookings, setBookings] = useState([]);

	//const { user } = useAuth();

	const user = {
		userId: "12345", // Mock user ID for demonstration purposes
	};

	const fakeBookings = [
		{ id: "1", slotId: "A1", date: "2023-10-01T00:00:00", status: 3 },
		{ id: "2", slotId: "A2", date: "2023-10-02T00:00:00", status: 1 },
		{ id: "3", slotId: "A3", date: "2023-10-03T00:00:00", status: 2 },
		{ id: "4", slotId: "A3", date: "2023-10-04T00:00:00", status: 0 },
		{ id: "5", slotId: "A1", date: "2026-10-01T00:00:00", status: 3 },
		{ id: "6", slotId: "A2", date: "2026-10-02T00:00:00", status: 1 },
		{ id: "7", slotId: "A3", date: "2026-10-03T00:00:00", status: 2 },
		{ id: "8", slotId: "A3", date: "2026-10-04T00:00:00", status: 0 },
	];

	useEffect(() => {
		if (user && user.userId && bookings.length === 0) {
			BookingService.getBookingsByUserId(user.userId)
				.then((response) => {
					setBookings(response);
				})
				.catch((error) => {
					console.error("Failed to fetch bookings:", error);
					setBookings(fakeBookings); // Use fake data for demonstration
				});
		}
	}, [user]);

	const today = new Date();
	today.setHours(0, 0, 0, 0);

	const futureBookings = bookings.filter((b) => new Date(b.date) >= today);
	const pastBookings = bookings.filter((b) => new Date(b.date) < today);

	const handleCancelBooking = (bookingId: string) => {
		BookingService.deleteBooking(bookingId)
			.then(() => {
				setBookings((prevBookings) =>
					prevBookings.map((booking) =>
						booking.id === bookingId
							? { ...booking, status: BookingStatus.CANCELLED }
							: booking
					)
				);
			})
			.catch((error) => {
				console.error("Failed to cancel booking:", error);
				alert("Erreur lors de l'annulation de la réservation.");
				setBookings((prevBookings) =>
					prevBookings.map((booking) =>
						booking.id === bookingId
							? { ...booking, status: BookingStatus.CANCELLED }
							: booking
					)
				);
			});
	};

	const renderSection = (title: string, list: typeof bookings) => (
		<div className="mb-6">
			<h3 className="text-xl font-semibold mb-4 text-gray-700">{title}</h3>
			<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
				{list.map((booking) => {
					return (
						<HistoryBookingItem
							booking={booking}
							onCancel={handleCancelBooking}
						/>
					);
				})}
			</div>
		</div>
	);

	return (
		<div className="p-6 max-w-6xl mx-auto">
			<h2 className="text-3xl font-bold mb-8">Historique de Réservation</h2>
			{renderSection("Réservations à venir", futureBookings)}
			{renderSection("Réservations passées", pastBookings)}
		</div>
	);
}
