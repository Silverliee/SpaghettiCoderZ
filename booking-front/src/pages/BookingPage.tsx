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
import { format } from "date-fns";

import BookingService from "@/services/bookingService";
import { BookingStatus, ParkingSlot, UserRole } from "@/interface/interface";
import ParkingService from "@/services/parkingService";
import ParkingMap from "@/components/Parking/ParkingMap";
import { useAuth } from "@/contexts/AuthContext";
import { max } from "date-fns";
import { toast } from "sonner";
import { formatDate } from "@/lib/utils";

export default function BookingPage() {
	const [selectedDate, setSelectedDate] = useState<Date | undefined>(
		new Date()
	);
	const [parkingSlots, setParkingSlots] = useState<ParkingSlot[]>([]);
	const [hasAlreadyBooked, setHasAlreadyBooked] = useState<boolean>(false);
	const [numberOfCurrentBookings, setNumberOfCurrentBookings] =
		useState<number>(0);
	const [maxQuotaBooking, setMaxQuotaBooking] = useState<number>(5);
	const { user } = useAuth();
	const [todayBooking, setTodayBooking] = useState<{
		id: string;
		status: BookingStatus;
	} | null>(null);

	useEffect(() => {
		if (!selectedDate || !user) return;

		const selectedDateFormatted = format(selectedDate, "yyyy-MM-dd");

		BookingService.getBookingsByUserId(user.userId)
			.then((bookings) => {
				// Vérifie si l'une des réservations correspond à la date sélectionnée
				const matchingBooking = bookings.find((booking) => {
					const bookingDate = new Date(booking.date); // ou booking.bookingDate selon ton interface
					const bookingDateFormatted = format(bookingDate, "yyyy-MM-dd");
					return bookingDateFormatted === selectedDateFormatted;
				});

				if (matchingBooking) {
					setTodayBooking({
						id: matchingBooking.id,
						status: matchingBooking.status,
					});
				} else {
					setTodayBooking(null);
				}
			})
			.catch((err) => {
				toast.error(
					"Erreur lors de la récupération des réservations.Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.",
					{
						duration: 2000,
					}
				);
				setTodayBooking(null);
			});
	}, [selectedDate, user]);

	useEffect(() => {
		if (user) {
			let MAX_QUOTA_BOOKINGS = 0;
			if (user.role === UserRole.MANAGER) {
				MAX_QUOTA_BOOKINGS = 10; // Managers can book up to 10 slots
				setMaxQuotaBooking(MAX_QUOTA_BOOKINGS);
			}
			BookingService.getBookingsByUserId(user.userId)
				.then((bookings) => {
					const currentBookings = bookings.filter(
						(booking) => booking.status === BookingStatus.BOOKED
					);
					setNumberOfCurrentBookings(currentBookings.length);
				})
				.catch((err) => {
					toast.error(
						"Erreur lors de la récupération des réservations. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.",
						{
							duration: 2000,
						}
					);
				});
		}
	}, [user]);

	useEffect(() => {
		const userHasABookingOnThisDate = parkingSlots.some(
			(slot) =>
				slot.isBooked &&
				typeof slot.userId !== "undefined" &&
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
			.catch((err) =>
				toast.error(
					"Erreur lors de la récupération des places du parking. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.",
					{
						duration: 2000,
					}
				)
			);
	}, [selectedDate]);

	function handleBookParkingSlot(slotId: string) {
		if (!selectedDate) return;
		const localDate = new Date(selectedDate);
		const utcDate = new Date(
			localDate.getTime() - localDate.getTimezoneOffset() * 60000
		);
		BookingService.bookParkingSlotPerDate(slotId, utcDate, user?.userId)
			.then(() => {
				setParkingSlots((prevSlots) =>
					prevSlots.map((slot) =>
						slot.id === slotId
							? { ...slot, isBooked: true, userId: user.userId }
							: slot
					)
				);
				toast.success(
					`Votre place de parking a bien été réservée pour le ${formatDate(
						utcDate
					)}.`,
					{
						duration: 2000,
					}
				);
				setNumberOfCurrentBookings((previous) => {
					return previous + 1;
				});
				setHasAlreadyBooked(true);
			})
			.catch((error) => {
				toast.error(
					`Une erreur s'est produite lors de votre réservation. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
					{
						duration: 2000,
					}
				);
			});
	}

	function handleCheckIn(bookingId: string) {
		BookingService.checkInBooking(bookingId, user?.userId)
			.then(() => {
				setTodayBooking((prev) =>
					prev ? { ...prev, status: BookingStatus.COMPLETED } : null
				);
				setHasAlreadyBooked(true);
				toast.success(`Votre enregistrement a été effectué avec succès.`, {
					duration: 2000,
				});
			})
			.catch((error) =>
				toast.error(
					`Une erreur s'est produite lors de votre enreigstrement. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
					{
						duration: 2000,
					}
				)
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
					{hasAlreadyBooked &&
						todayBooking?.status === BookingStatus.BOOKED && (
							<div className="text-red-600 font-medium text-center border border-red-300 bg-red-50 p-3 rounded-md">
								Vous avez déjà une réservation pour cette date.
							</div>
						)}
					{numberOfCurrentBookings === maxQuotaBooking && (
						<div className="text-red-600 font-medium text-center border border-red-300 bg-red-50 p-3 rounded-md">
							Vous avez atteind votre nombre de réservations maximum.
						</div>
					)}
					<div>
						{selectedDate &&
							new Date().toDateString() ===
								new Date(selectedDate).toDateString() &&
							todayBooking && (
								<div className="text-center">
									{todayBooking.status === BookingStatus.BOOKED ? (
										<button
											onClick={() => handleCheckIn(todayBooking.id)}
											className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition"
										>
											Faire le Check-in
										</button>
									) : todayBooking.status === BookingStatus.COMPLETED ? (
										<div className="text-green-600 font-medium border border-green-300 bg-green-50 p-3 rounded-md inline-block">
											Check-in effectué ✅
										</div>
									) : null}
								</div>
							)}
						<br />

						<ParkingMap
							parkingSlots={parkingSlots}
							handleBookParkingSlot={handleBookParkingSlot}
							hasAlreadyBooked={
								hasAlreadyBooked || numberOfCurrentBookings === maxQuotaBooking
							}
						/>
					</div>
				</CardContent>
			</Card>
		</div>
	);
}
