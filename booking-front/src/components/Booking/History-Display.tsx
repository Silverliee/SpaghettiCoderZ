import { useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";
import BookingService from "@/services/bookingService";
import { BookingStatus } from "@/interface/interface";
import HistoryBookingItem from "./history-booking-item";
import { toast } from "sonner";

export default function HistoryDisplay() {
	const [bookings, setBookings] = useState([]);

	const { user } = useAuth();

	useEffect(() => {
		if (user && user.userId && bookings.length === 0) {
			BookingService.getBookingsByUserId(user.userId)
				.then((response) => {
					setBookings(response);
				})
				.catch((error) => {
					toast.error(
						`Une erreur s'est produite lors de la récupération de l'historique. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
						{
							duration: 2000,
						}
					);
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
				toast.error("Réservation annulée avec success.", {
					duration: 2000,
				});
				setBookings((prevBookings) =>
					prevBookings.map((booking) =>
						booking.id === bookingId
							? { ...booking, status: BookingStatus.CANCELLED }
							: booking
					)
				);
			})
			.catch((error) => {
				toast.error("Erreur lors de l'annulation de la réservation.", {
					duration: 2000,
				});
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
