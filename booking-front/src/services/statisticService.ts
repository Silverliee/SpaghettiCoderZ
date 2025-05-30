import { formatDate } from "@/lib/utils";
import AxiosInstance from "./AxiosInstance";

export default class StatisticService {
	public static async getGlobalStatistics() {
		const response = await AxiosInstance.get(`/Statistic/global`);
		return response.data;
	}

	public static async getGlobalStatisticsPerDate(
		startDate: Date,
		endDate: Date
	) {
		const response = await AxiosInstance.post(`/Statistic/global/period`, {
			startDate: formatDate(startDate),
			endDate: formatDate(endDate),
		});
		return response.data;
	}

	public static async getGlobalStatisticsPerUserId(userId: number) {
		const response = await AxiosInstance.get(`/Statistic/user/${userId}`);
		return response.data;
	}

	public static async getGlobalStatisticsPerDatePerUserId(
		userId: number,
		startDate: Date,
		endDate: Date
	) {
		const response = await AxiosInstance.get(
			`/Statistic/user/${userId}/period?startDate=${formatDate(
				startDate
			)}&endDate=${formatDate(endDate)}`
		);
		return response.data;
	}
}
