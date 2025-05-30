import { Button } from "@/components/ui/button";
import { useAuth } from "@/contexts/AuthContext";

export default function ParkingMap({
	parkingSlots,
	handleBookParkingSlot,
	hasAlreadyBooked,
}) {
	const rows = ["A", "B", "C", "D", "E", "F"];
	const columns = Array.from({ length: 10 }, (_, i) => i + 1);

	const getSlot = (row: string, column: number) =>
		parkingSlots.find((slot) => slot.row === row && slot.column === column);

	const { user } = useAuth();

	return (
		<div className="flex flex-col items-center gap-6 bg-black p-6 rounded-lg shadow-lg">
			<h2 className="text-white text-2xl font-semibold">Plan du Parking</h2>

			<div className="flex flex-col gap-2">
				{rows.map((row) => (
					<div key={row} className="flex gap-2 items-center">
						{columns.map((col) => {
							const slot = getSlot(row, col);
							if (!slot)
								return <div key={`${row}-${col}`} className="w-12 h-12" />;

							//const isMine = slot.userId === user.userId;
							const isMine = slot.userId === "me";
							const label = `${row}${col}`;
							const isDisabled =
								hasAlreadyBooked || slot.isBooked || slot.inMaintenance;

							let bg = "bg-gray-100 text-black"; // disponible
							if (slot.isBooked) bg = "bg-gray-500 text-white";
							if (slot.inMaintenance) bg = "bg-yellow-400 text-black";
							if (isMine) bg = "bg-blue-500 text-white";

							const border = slot.hasCharger
								? "border-2 border-green-500"
								: isMine
								? "border-2 border-blue-500"
								: "";
							return (
								<Button
									key={slot.id}
									disabled={isDisabled}
									onClick={() => handleBookParkingSlot(slot.id)}
									className={`w-12 h-12 p-0 text-xs font-semibold ${bg} ${border}`}
									title={
										slot.inMaintenance
											? "En maintenance"
											: slot.isBooked
											? "Réservé"
											: slot.hasCharger
											? "Borne de recharge"
											: "Disponible"
									}
								>
									{label}
								</Button>
							);
						})}
					</div>
				))}
			</div>

			{/* Légende */}
			<div className="mt-4 text-sm text-white flex gap-4 flex-wrap">
				<div className="flex items-center gap-2">
					<div className="w-4 h-4 bg-gray-100 border" />
					<span>Disponible</span>
				</div>
				<div className="flex items-center gap-2">
					<div className="w-4 h-4 bg-gray-500" />
					<span>Réservé</span>
				</div>
				<div className="flex items-center gap-2">
					<div className="w-4 h-4 bg-yellow-400" />
					<span>Maintenance</span>
				</div>
				<div className="flex items-center gap-2">
					<div className="w-4 h-4 border-2 border-green-500" />
					<span>Chargeur</span>
				</div>
				<div className="flex items-center gap-2">
					<div className="w-4 h-4 border-2 bg-blue-500" />
					<span>Ma réservation</span>
				</div>
			</div>
		</div>
	);
}
