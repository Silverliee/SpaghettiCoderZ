import axios from "axios";
import { LoginPayload, AuthResponse } from "../interface/interface";

export const login = async (payload: LoginPayload): Promise<AuthResponse> => {
	const response = await axios.post<AuthResponse>(
		`${process.env.API_BACK_URL}/login`,
		payload
	);
	return response.data;
};
