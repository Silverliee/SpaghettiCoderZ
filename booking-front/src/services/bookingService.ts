import { Booking, BookingStatus, ParkingSlot } from "../interface/interface";
import { formatDate } from "@/lib/utils";
import AxiosInstance from "@/services/AxiosInstance";

export default class BookingService {
	public static async bookParkingSlotPerDate(
		parkingSlotId: string,
		date: Date,
		userId: number
	) {
		console.log(`Booking parking slot ${parkingSlotId} for date ${date}`);
		const response = await AxiosInstance.post(`/Booking`, {
			slotId: parkingSlotId,
			date: formatDate(date),
			status: BookingStatus.BOOKED,
			userId,
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
		const response = await AxiosInstance.post(`/Booking/bulk`, bookingsPayload);

		return response.data;
	}
	public static async getBookings(): Promise<Booking[]> {
		// Logic to cancel a booking
		const bookings = await AxiosInstance.get(`/Booking`);
		return bookings.data;
	}
	public static async getBookingById(bookingId: string): Promise<Booking> {
		const bookings = await AxiosInstance.get(`/Booking/${bookingId}`);
		return bookings.data;
	}
	public static async updateBooking(updateBooking: Booking): Promise<Booking> {
		const response = await AxiosInstance.put(`/Booking/${updateBooking.id}`, {
			slotId: updateBooking.parkingSlotId,
			date: formatDate(updateBooking.date),
			status: updateBooking.status,
		});
		return response.data;
	}
	public static async deleteBooking(bookingId: string): Promise<Booking> {
		const response = await AxiosInstance.delete(`/Booking/${bookingId}`);
		return response.data;
	}
	public static async getBookingsPerDate(date: Date): Promise<Booking[]> {
		const response = await AxiosInstance.get(
			`/Booking/date/${formatDate(date)}`
		);
		return response.data;
	}
	public static async getBookingsByUserId(userId: number) {
		const response = await AxiosInstance.get(`/Booking/user/${userId}`);
		return response.data;
	}

	public static async checkInBooking(bookingId, userId) {
		const response = await AxiosInstance.post(`/Booking/checkin`, {
			bookingId,
			userId,
		});
		return response.data;
	}
}
