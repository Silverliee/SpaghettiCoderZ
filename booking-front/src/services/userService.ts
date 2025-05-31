import AxiosInstance from "./AxiosInstance";

export default class UserService {
	public static async getAll() {
		const response = await AxiosInstance.get("/User");
		return response.data;
	}
}
