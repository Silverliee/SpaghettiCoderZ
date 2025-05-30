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
import { BookingStatus, ParkingSlot } from "@/interface/interface";
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
	const MAX_QUOTA_BOOKINGS = user?.role === "USER" ? 5 : 30;

	useEffect(() => {
		if (user) {
			BookingService.getBookingsByUserId(user.userId).then((bookings) => {
				console.log("Current bookings:", bookings);
				const currentBookings = bookings.filter(
					(booking) => booking.status === BookingStatus.BOOKED
				);
				setNumberOfCurrentBookings(currentBookings.length);
			});
		} else {
			const fakeBookings = [
				{
					id: "1",
					slotId: "A1",
					date: "2023-10-01T00:00:00",
					status: 3,
					userId: "me",
				},
				{
					id: "2",
					slotId: "A2",
					date: "2023-10-02T00:00:00",
					status: 1,
					userId: "me",
				},
				{
					id: "3",
					slotId: "A3",
					date: "2023-10-03T00:00:00",
					status: 2,
					userId: "me",
				},
				{
					id: "4",
					slotId: "A3",
					date: "2023-10-04T00:00:00",
					status: 0,
					userId: "me",
				},
				{
					id: "5",
					slotId: "A1",
					date: "2026-10-01T00:00:00",
					status: 3,
					userId: "me",
				},
				{
					id: "6",
					slotId: "A2",
					date: "2026-10-02T00:00:00",
					status: 1,
					userId: "me",
				},
				{
					id: "7",
					slotId: "A3",
					date: "2026-10-03T00:00:00",
					status: 2,
					userId: "me",
				},
				{
					id: "8",
					slotId: "A3",
					date: "2026-10-04T00:00:00",
					status: 0,
					userId: "me",
				},
			];

			const currentBookings = fakeBookings.filter(
				(booking) => booking.status === BookingStatus.BOOKED
			);
			setNumberOfCurrentBookings(currentBookings.length);
		}
	}, [user]);

	useEffect(() => {
		console.log("numberOfCurrentBookings: " + numberOfCurrentBookings);
	}, [numberOfCurrentBookings]);

	useEffect(() => {
		if (!user) {
			const storedUser = localStorage.getItem("user");
			if (storedUser) {
				setUser(JSON.parse(storedUser));
			}
		}
		const userHasABookingOnThisDate = parkingSlots.some(
			(slot) =>
				slot.isBooked &&
				typeof slot.userId !== "undefined" &&
				//slot?.userId === user?.id
				slot?.userId === "me"
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
				//console.log("Places disponibles :", response);
				// const fakeResponse = response.map((slot) => {
				// 	if (slot.id == 1) {
				// 		return {
				// 			id: 1,
				// 			row: "A",
				// 			bookingId: 1,
				// 			column: 1,
				// 			hasCharger: true,
				// 			inMaintenance: false,
				// 			isBooked: true,
				// 			userId: "me",
				// 		};
				// 	} else {
				// 		return slot;
				// 	}
				// });
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

		BookingService.bookParkingSlotPerDate(slotId, utcDate)
			.then(() => {
				setParkingSlots((prevSlots) =>
					prevSlots.map((slot) =>
						slot.id === slotId ? { ...slot, isBooked: true } : slot
					)
				);
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
							hasAlreadyBooked={hasAlreadyBooked}
						/>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
