import axios from "axios";

const AxiosInstance = axios.create({
	baseURL: import.meta.env.VITE_API_BACK_URL,
});

export default AxiosInstance;
