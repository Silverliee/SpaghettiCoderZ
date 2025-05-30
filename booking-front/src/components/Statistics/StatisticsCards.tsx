import { CalendarDays, Car, Percent, XCircle, UserX } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

type StatisticsOld = {
	canceledBookingsCount: number;
	currentAvailableParkingSlots: number;
	noShowCount: number;
	occupancyRatePercentage: number;
	peakBookingDate: string;
};

type StatisticsNew = {
	bookingsCount: number;
	cancellationsCount: number;
	lastBookingDate: string;
	noShowsCount: number;
	userId: number;
};

type Props = {
	data: StatisticsOld | StatisticsNew;
};

export function StatisticsCards({ data }: Props) {
	// On détecte le format avec une propriété spécifique
	const isOldFormat = "canceledBookingsCount" in data;

	const statsOld = [
		{
			title: "Annulations",
			icon: <XCircle className="text-red-500" size={24} />,
			value: (data as StatisticsOld).canceledBookingsCount,
		},
		{
			title: "No-shows",
			icon: <UserX className="text-yellow-500" size={24} />,
			value: (data as StatisticsOld).noShowCount,
		},
		{
			title: "Places disponibles",
			icon: <Car className="text-green-600" size={24} />,
			value: (data as StatisticsOld).currentAvailableParkingSlots,
		},
		{
			title: "Taux d’occupation",
			icon: <Percent className="text-blue-500" size={24} />,
			value: `${(data as StatisticsOld).occupancyRatePercentage}%`,
		},
		{
			title: "Jour le + réservé",
			icon: <CalendarDays className="text-purple-500" size={24} />,
			value: new Date(
				(data as StatisticsOld).peakBookingDate
			).toLocaleDateString("fr-FR"),
		},
	];

	const statsNew = [
		{
			title: "Réservations",
			icon: <CalendarDays className="text-blue-500" size={24} />,
			value: (data as StatisticsNew).bookingsCount,
		},
		{
			title: "Annulations",
			icon: <XCircle className="text-red-500" size={24} />,
			value: (data as StatisticsNew).cancellationsCount,
		},
		{
			title: "No-shows",
			icon: <UserX className="text-yellow-500" size={24} />,
			value: (data as StatisticsNew).noShowsCount,
		},
		{
			title: "Dernière réservation",
			icon: <CalendarDays className="text-purple-500" size={24} />,
			value: new Date(
				(data as StatisticsNew).lastBookingDate
			).toLocaleDateString("fr-FR"),
		},
	];

	const statsToDisplay = isOldFormat ? statsOld : statsNew;

	return (
		<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 w-full max-w-5xl">
			{statsToDisplay.map((stat, idx) => (
				<Card key={idx} className="shadow-md rounded-2xl border">
					<CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
						<CardTitle className="text-sm font-medium">{stat.title}</CardTitle>
						{stat.icon}
					</CardHeader>
					<CardContent>
						<p className="text-2xl font-bold">{stat.value}</p>
					</CardContent>
				</Card>
			))}
		</div>
	);
}
