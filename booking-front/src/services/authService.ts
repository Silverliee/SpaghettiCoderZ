import axios from "axios";
import {
	LoginPayload,
	AuthResponse,
	RegisterPayload,
	RegisterResponse,
} from "../interface/interface";
import AxiosInstance from "./AxiosInstance";

const API_BACK_URL = import.meta.env.VITE_API_BACK_URL;

export default class AuthService {
	public static async login(payload: LoginPayload): Promise<AuthResponse> {
		const response = await axios.post<AuthResponse>(
			`${API_BACK_URL}/User/login`,
			payload
		);
		return response.data;
	}

	public static async register(
		payload: RegisterPayload
	): Promise<RegisterResponse> {
		const response = await axios.post<RegisterResponse>(
			`${API_BACK_URL}/User/register`,
			payload
		);
		return response.data;
	}

	public static async logout(): Promise<void> {
		await AxiosInstance.post(`/User/logout`, {});
	}
}
