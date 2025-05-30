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
import { ParkingSlot } from "@/interface/interface";
import ParkingService from "@/services/parkingService";
import ParkingMap from "@/components/Parking/ParkingMap";

export default function BookingPage() {
	const [selectedDate, setSelectedDate] = useState<Date | undefined>(
		new Date()
	);
	const [parkingSlots, setParkingSlots] = useState<ParkingSlot[]>([]);
	const [user, setUser] = useState(null);
	const [hasAlreadyBooked, setHasAlreadyBooked] = useState<boolean>(false);

	useEffect(() => {
		const storedUser = localStorage.getItem("user");
		if (storedUser) {
			const userData = JSON.parse(storedUser);
			setUser(userData);
			setHasAlreadyBooked(userData.hasAlreadyBooked || false);
		}
	}, [parkingSlots]);

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
				slot?.userId === user?.id
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
