import { useState, useEffect } from "react";
import {
	Card,
	CardHeader,
	CardTitle,
	CardDescription,
	CardContent,
} from "@/components/ui/card";
import { Calendar } from "@/components/ui/calendar";
import { Label } from "@/components/ui/label";

import BookingService from "@/services/bookingService";
import { BookingStatus, ParkingSlot, UserRole } from "@/interface/interface";
import ParkingService from "@/services/parkingService";
import ParkingMap from "@/components/Parking/ParkingMap";
import { useAuth } from "@/contexts/AuthContext";

export default function BookingPage() {
	const [selectedDate, setSelectedDate] = useState<Date | undefined>(
		new Date()
	);
	const [parkingSlots, setParkingSlots] = useState<ParkingSlot[]>([]);
	const [hasAlreadyBooked, setHasAlreadyBooked] = useState<boolean>(false);
	const [numberOfCurrentBookings, setNumberOfCurrentBookings] =
		useState<number>(0);

	const { user } = useAuth();
	const MAX_QUOTA_BOOKINGS = user?.role === UserRole.MANAGER ? 30 : 5;

	useEffect(() => {
		if (user) {
			BookingService.getBookingsByUserId(user.userId).then((bookings) => {
				console.log("Current bookings:", bookings);
				const currentBookings = bookings.filter(
					(booking) => booking.status === BookingStatus.BOOKED
				);
				setNumberOfCurrentBookings(currentBookings.length);
			});
		}
		console.log("MAX_QUOTA_BOOKINGS" + MAX_QUOTA_BOOKINGS);
		console.log("user role: " + user?.role);
	}, [user]);

	useEffect(() => {
		console.log("numberOfCurrentBookings: " + numberOfCurrentBookings);
	}, [numberOfCurrentBookings]);

	useEffect(() => {
		console.log("parkingSlots:", parkingSlots);
		const userHasABookingOnThisDate = parkingSlots.some(
			(slot) =>
				slot.isBooked &&
				typeof slot.userId !== "undefined" &&
				//slot?.userId === user?.id
				slot?.userId === user?.userId
		);
		setHasAlreadyBooked(userHasABookingOnThisDate);
	}, [parkingSlots]);

	useEffect(() => {
		if (!selectedDate) return;
		const localDate = new Date(selectedDate);
		const utcDate = new Date(
			localDate.getTime() - localDate.getTimezoneOffset() * 60000
		);

		console.log("Fetching parking slots for date:", utcDate);
		ParkingService.getParkingStatePerDate(utcDate)
			.then((response) => {
				setParkingSlots(response);
			})
			.catch((error) =>
				console.error("Erreur de récupération des places :", error)
			);
	}, [selectedDate]);

	function handleBookParkingSlot(slotId: string) {
		if (!selectedDate) return;
		const localDate = new Date(selectedDate);
		const utcDate = new Date(
			localDate.getTime() - localDate.getTimezoneOffset() * 60000
		);
		console.log(`Réservation du slot ID: ${slotId}`);
		console.log("User ID:", user);
		BookingService.bookParkingSlotPerDate(slotId, utcDate, user?.userId)
			.then(() => {
				console.log("coucou");
				setParkingSlots((prevSlots) =>
					prevSlots.map((slot) =>
						slot.id === slotId
							? { ...slot, isBooked: true, userId: user.userId }
							: slot
					)
				);
				setHasAlreadyBooked(true);
			})
			.catch((error) =>
				console.error("Erreur lors de la réservation :", error)
			);
	}

	return (
		<div className="p-6">
			<Card>
				<CardHeader>
					<CardTitle>Réserver une place</CardTitle>
					<CardDescription>
						Choisissez une date pour voir les places disponibles
					</CardDescription>
				</CardHeader>
				<CardContent className="flex flex-col gap-6">
					<div>
						<Label className="mb-2 block">Date :</Label>
						<div className="flex justify-center">
							<div className="w-fit">
								<Calendar
									mode="single"
									selected={selectedDate}
									onSelect={setSelectedDate}
									className="rounded-md border"
								/>
							</div>
						</div>
					</div>
					{hasAlreadyBooked && (
						<div className="text-red-600 font-medium text-center border border-red-300 bg-red-50 p-3 rounded-md">
							Vous avez déjà une réservation pour cette date.
						</div>
					)}
					{numberOfCurrentBookings === MAX_QUOTA_BOOKINGS && (
						<div className="text-red-600 font-medium text-center border border-red-300 bg-red-50 p-3 rounded-md">
							Vous avez atteind votre nombre de réservations maximum.
						</div>
					)}
					<div>
						<ParkingMap
							parkingSlots={parkingSlots}
							handleBookParkingSlot={handleBookParkingSlot}
							hasAlreadyBooked={
								hasAlreadyBooked ||
								numberOfCurrentBookings === MAX_QUOTA_BOOKINGS
							}
						/>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
