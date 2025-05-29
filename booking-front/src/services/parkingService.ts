import axios from "axios";
import BookingService from "./bookingService";
import Booking from "@/components/Booking/Booking";
import { BookingStatus } from "@/interface/interface";

const API_BACK_URL = import.meta.env.VITE_API_BACK_URL;

export default class ParkingService {
	public static async getParking() {
		const response = await axios.get(`${API_BACK_URL}/Parking`);
		return response.data;
	}

	public static async getParkingStatePerDate(date: Date) {
		const parking = await this.getParking();
		const bookings = await BookingService.getBookingsPerDate(date);
		console.log("Bookings for date:", bookings);
		console.log("Parking slots:", parking);
		const parkingSlots = parking.map((slot) => {
			const booking = bookings.find((booking) => booking.slotId === slot.id);
			return {
				...slot,
				isBooked: !!(booking?.status === BookingStatus.BOOKED),
				bookingId: booking ? booking.id : null,
			};
		});
		return parkingSlots;
	}
}
