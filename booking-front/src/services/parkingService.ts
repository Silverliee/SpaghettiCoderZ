import BookingService from "./bookingService";
import { BookingStatus } from "@/interface/interface";
import AxiosInstance from "./AxiosInstance";
import { toast } from "sonner";

export default class ParkingService {
	public static async getParking() {
		const response = await AxiosInstance.get(`/Parking`);
		return response.data;
	}

	public static async getParkingStatePerDate(date: Date) {
		const parking = await this.getParking().catch((err) => {
			toast.error(
				`Une erreur s'est produite lors de la récupération du parking. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
				{
					duration: 2000,
				}
			);
			return [];
		});
		const bookings = await BookingService.getBookingsPerDate(date).catch(
			(err) => {
				toast.error(
					`Une erreur s'est produite lors des réservations du jour. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
					{
						duration: 2000,
					}
				);
				return [];
			}
		);
		const parkingSlots = parking.map((slot) => {
			const booking = bookings.find(
				(booking) => booking.slotId === Number(slot.id)
			);
			const status = booking?.status;
			const bookingId = booking?.id;
			const userId = booking?.userId;
			return {
				...slot,
				isBooked:
					status == BookingStatus.BOOKED || status == BookingStatus.COMPLETED,
				bookingId,
				userId,
			};
		});

		return parkingSlots;
	}
}
