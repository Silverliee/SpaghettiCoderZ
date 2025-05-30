import axios from "axios";

const AxiosInstance = axios.create({
	baseURL: import.meta.env.VITE_API_BACK_URL,
});

AxiosInstance.interceptors.request.use((config) => {
	const auth = localStorage.getItem("user");
	const token = auth ? JSON.parse(auth).token : null;

	if (token) {
		config.headers.Authorization = `Bearer ${token}`;
	}
	return config;
});

export default AxiosInstance;
