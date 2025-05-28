import axios from "axios";
import { Booking, ParkingSlot } from "../interface/interface";

export default class BookingService {
	private static instance: BookingService;

	public static async bookParkingSlotPerDate(
		parkingSlotId: string,
		date: Date
	): Promise<void> {
		// Logic to book a parking slot
		// axios.post(`${process.env.REACT_APP_API_BACK_URL}/Reservation`, {
		//   parkingSlotId,
		//   date,
		// });

		console.log(`Booking parking slot ${parkingSlotId} for date ${date}`);
	}

	public static async cancelBooking(bookingId: string): Promise<void> {
		// Logic to book a parking slot
		// axios.delete(`${process.env.REACT_APP_API_BACK_URL}/Reservation`, {
		//   parkingSlotId,
		//   date,
		// });

		console.log(`Cancel Booking ${bookingId}`);
	}

	public static async getBookings(): Promise<Booking[]> {
		// Logic to cancel a booking
		console.log(`Requesting bookings for user`);
		//axios.get(`${process.env.REACT_APP_API_BACK_URL}/Reservation`);
		return [];
	}

	public static async getParkingStatePerDate(
		date: Date
	): Promise<ParkingSlot[]> {
		// get all parking slots (parking) and all bookings for a specific date
		console.log(`Requesting parking state for date ${date}`);
		//axios.get(`${process.env.REACT_APP_API_BACK_URL}/Reservation`);
		return [
			{
				id: "1",
				hasCharger: true,
			},
		];
	}
}
