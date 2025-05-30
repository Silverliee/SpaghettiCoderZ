import AxiosInstance from "./AxiosInstance";

export default class UserService {
	private static instance: UserService;

	public static async getUserById(userId: number): Promise<void> {
		// Logic to book a parking slot
		// AxiosInstance.post(`/Reservation`, {
		//   parkingSlotId,
		//   date,
		// });

		console.log(`Booking parking slot ${parkingSlotId} for date ${date}`);
	}
}
