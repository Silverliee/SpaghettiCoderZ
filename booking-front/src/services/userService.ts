import axios from "axios";
const API_BACK_URL = import.meta.env.VITE_API_BACK_URL;

export default class UserService {
  private static instance: UserService;

  public static async getUserById(
    userId: number,
  ): Promise<void> {
    // Logic to book a parking slot
    // axios.post(`${API_BACK_URL}/Reservation`, {
    //   parkingSlotId,
    //   date,
    // });

    console.log(`Booking parking slot ${parkingSlotId} for date ${date}`);
  }
}