import BookingService from "./bookingService";
import { BookingStatus } from "@/interface/interface";
import AxiosInstance from "./AxiosInstance";

export default class ParkingService {
	public static async getParking() {
		const response = await AxiosInstance.get(`/Parking`);
		return response.data;
	}

	public static async getParkingStatePerDate(date: Date) {
		const parking = await this.getParking();
		const bookings = await BookingService.getBookingsPerDate(date);
		console.log("Bookings for date:", bookings);
		console.log("Parking for date:", parking);
		const parkingSlots = parking.map((slot) => {
			const booking = bookings.find(
				(booking) => booking.slotId === Number(slot.id)
			);
			const status = booking?.status;
			const bookingId = booking?.id;
			const userId = booking?.userId;
			console.log(
				`Slot ${slot.id} - Booking ID: ${bookingId}, User ID: ${userId}, Status: ${status}`
			);
			return {
				...slot,
				isBooked: status == BookingStatus.BOOKED,
				bookingId,
				userId,
			};
		});
		console.log("ParkingSlots for date:", parkingSlots);

		return parkingSlots;
	}
}
