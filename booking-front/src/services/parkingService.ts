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
		const parkingSlots = parking.map((slot) => {
			const booking = bookings.find((booking) => booking.slotId === slot.id);
			return {
				...slot,
				isBooked: !!(booking?.status === BookingStatus.BOOKED),
				bookingId: booking ? booking.id : null,
				userId: booking ? booking.userId : null,
			};
		});
		console.log("ParkingSlots for date:", parkingSlots);

		return parkingSlots;
	}
}
