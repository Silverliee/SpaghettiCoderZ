import { use, useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { DateRange } from "react-day-picker";
import { DateRangePicker } from "@/components/ui/DatePickerRange";
import StatisticService from "@/services/statisticService";
import UserService from "@/services/userService";
import { set } from "date-fns";
import { StatisticsCards } from "@/components/Statistics/StatisticsCards";
import { toast } from "sonner";

export default function StatisTicsPage() {
	const [showFilters, setShowFilters] = useState(false);
	const [users, setUsers] = useState<{ id: number; name: string }[]>([]);
	const [selectedUserId, setSelectedUserId] = useState<string>("");
	const [dateRange, setDateRange] = useState<DateRange | undefined>({
		from: new Date(),
		to: new Date(),
	});
	const [statsData, setStatsData] = useState<any>(null);

	useEffect(() => {
		if (showFilters) {
			if (selectedUserId) {
				StatisticService.getGlobalStatisticsPerDatePerUserId(
					parseInt(selectedUserId),
					dateRange?.from || new Date(),
					dateRange?.to || new Date()
				)
					.then((data) => {
						setStatsData(data);
					})
					.catch((error) => {
						toast.error(
							`Une erreur s'est produite lors de récupération des statistiques. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
							{
								duration: 2000,
							}
						);
					});
			} else {
				StatisticService.getGlobalStatisticsPerDate(
					dateRange?.from || new Date(),
					dateRange?.to || new Date()
				)
					.then((data) => {
						setStatsData(data);
					})
					.catch((error) => {
						toast.error(
							`Une erreur s'est produite lors de récupération des statistiques. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
							{
								duration: 2000,
							}
						);
					});
			}
		}
	}, [dateRange]);

	useEffect(() => {
		if (showFilters) {
			if (dateRange) {
				StatisticService.getGlobalStatisticsPerDatePerUserId(
					parseInt(selectedUserId),
					dateRange?.from || new Date(),
					dateRange?.to || new Date()
				)
					.then((data) => {
						setStatsData(data);
					})
					.catch((error) => {
						toast.error(
							`Une erreur s'est produite lors de récupération des statistiques. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
							{
								duration: 2000,
							}
						);
					});
			} else {
				StatisticService.getGlobalStatisticsPerUserId(parseInt(selectedUserId))
					.then((data) => {
						setStatsData(data);
					})
					.catch((error) => {
						toast.error(
							`Une erreur s'est produite lors de récupération des statistiques. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
							{
								duration: 2000,
							}
						);
					});
			}
		}
	}, [selectedUserId]);

	// Charger les stats initiales
	useEffect(() => {
		StatisticService.getGlobalStatistics()
			.then((data) => {
				setStatsData(data);
			})
			.catch((error) => {
				toast.error(
					`Une erreur s'est produite lors de récupération des statistiques. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
					{
						duration: 2000,
					}
				);
			});
	}, []);

	// Charger la liste des utilisateurs quand on affiche les filtres
	useEffect(() => {
		if (showFilters) {
			UserService.getAll()
				.then((data) => {
					setUsers(data);
				})
				.catch((err) => {
					toast.error(
						`Une erreur s'est produite lors de récupération des utilisateurs de l'entreprise. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
						{
							duration: 2000,
						}
					);
				});
		}
	}, [showFilters]);

	return (
		<div className="min-h-screen flex flex-col items-center justify-start bg-gray-100 p-6">
			<h1 className="text-2xl font-bold mb-4">StatisTicsPage</h1>

			<Button onClick={() => setShowFilters(!showFilters)}>
				{showFilters ? "Masquer les filtres" : "Afficher les filtres"}
			</Button>

			{showFilters && (
				<div className="mt-4 space-y-4 w-full max-w-md">
					{/* Sélecteur de plage de dates */}
					<DateRangePicker value={dateRange} onChange={setDateRange} />

					{/* Sélecteur d'utilisateur */}
					<Select value={selectedUserId} onValueChange={setSelectedUserId}>
						<SelectTrigger className="w-full">
							<SelectValue placeholder="Sélectionnez un utilisateur" />
						</SelectTrigger>
						<SelectContent>
							{users.map((user) => (
								<SelectItem key={user.id} value={user.id.toString()}>
									{user?.lastName} {user?.firstName}
								</SelectItem>
							))}
						</SelectContent>
					</Select>

					{/* 🔁 Bouton de réinitialisation des filtres */}
					<Button
						variant="outline"
						className="w-full"
						onClick={() => {
							setDateRange({ from: new Date(), to: new Date() });
							setSelectedUserId("");
						}}
					>
						Réinitialiser les filtres
					</Button>
				</div>
			)}

			<br />

			{statsData && <StatisticsCards data={statsData} />}
		</div>
	);
}
