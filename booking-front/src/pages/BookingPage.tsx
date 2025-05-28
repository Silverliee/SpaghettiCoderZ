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
// import BookingList from "@/components/Booking/BookingList"
import ParkingSlotList from "@/components/ParkingSlot/ParkingSlotList";
import { ParkingSlot } from "@/interface/interface";

export default function BookingPage() {
	const [selectedDate, setSelectedDate] = useState<Date | undefined>(
		new Date()
	);
	const [parkingSlots, setParkingSlots] = useState<ParkingSlot[]>([]);

	useEffect(() => {
		if (!selectedDate) return;

		BookingService.getParkingStatePerDate(selectedDate)
			.then((response) => {
				console.log("Places disponibles :", response);
				setParkingSlots(response);
			})
			.catch((error) =>
				console.error("Erreur de récupération des places :", error)
			);
	}, [selectedDate]);

	function handleBookParkingSlot(slotId: string) {
		if (!selectedDate) return;
		console.log(`Réservation du slot ID: ${slotId}`);

		BookingService.bookParkingSlotPerDate(slotId, selectedDate)
			.then(() => {
				setParkingSlots((prev) => prev.filter((slot) => slot.id !== slotId));
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
								<Label className="mb-2 block text-center">Date :</Label>
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
						<ParkingSlotList
							parkingSlots={parkingSlots}
							handleBookParkingSlot={handleBookParkingSlot}
						/>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
