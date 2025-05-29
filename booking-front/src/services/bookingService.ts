import axios from "axios";
import { Booking, BookingStatus, ParkingSlot } from "../interface/interface";
import { formatDate } from "@/lib/utils";
const API_BACK_URL = import.meta.env.VITE_API_BACK_URL;

export default class BookingService {
	public static async bookParkingSlotPerDate(
		parkingSlotId: string,
		date: Date
	) {
		console.log(`Booking parking slot ${parkingSlotId} for date ${date}`);
		const response = await axios.post(`${API_BACK_URL}/Booking`, {
			slotId: parkingSlotId,
			date: formatDate(date),
			status: BookingStatus.BOOKED,
		});

		return response.data;
	}
	public static async bookMultipleParkingSlots(
		bookings: {
			parkingSlotId: string;
			date: Date;
		}[]
	) {
		const bookingsPayload = bookings.map((booking) => ({
			slotId: booking.parkingSlotId,
			date: formatDate(booking.date),
			status: BookingStatus.BOOKED,
		}));
		console.log(`Bulk booking parking slots for dates`);
		const response = await axios.post(
			`${API_BACK_URL}/Booking/bulk`,
			bookingsPayload
		);

		return response.data;
	}
	public static async getBookings(): Promise<Booking[]> {
		// Logic to cancel a booking
		console.log(`Requesting bookings for user`);
		const bookings = await axios.get(`${API_BACK_URL}/Booking`);
		return bookings.data;
	}
	public static async getBookingById(bookingId: string): Promise<Booking> {
		console.log(`Requesting booking ${bookingId}`);
		const bookings = await axios.get(`${API_BACK_URL}/Booking/${bookingId}`);
		return bookings.data;
	}
	public static async updateBooking(updateBooking: Booking): Promise<Booking> {
		console.log(`Updating booking ${updateBooking.id}`);
		const response = await axios.put(
			`${API_BACK_URL}/Booking/${updateBooking.id}`,
			{
				slotId: updateBooking.parkingSlotId,
				date: formatDate(updateBooking.date),
				status: updateBooking.status,
			}
		);
		return response.data;
	}
	public static async deleteBooking(bookingId: string): Promise<Booking> {
		console.log(`Deleteing booking ${bookingId}`);
		const response = await axios.delete(`${API_BACK_URL}/Booking/${bookingId}`);
		return response.data;
	}

	public static async getParkingStatePerDate(
		date: Date
	): Promise<ParkingSlot[]> {
		// get all parking slots (parking) and all bookings for a specific date
		console.log(`Requesting parking state for date ${formatDate(date)}`);
		//axios.get(`${API_BACK_URL}/Reservation`);
		return [
			{
				id: "1",
				hasCharger: true,
			},
		];
	}

	public static async getBookingsPerDate(date: Date): Promise<Booking[]> {
		console.log(`Requesting bookings for date ${formatDate(date)}`);
		const response = await axios.get(
			`${API_BACK_URL}/Booking/date/${formatDate(date)}`
		);
		return response.data;
	}
}
