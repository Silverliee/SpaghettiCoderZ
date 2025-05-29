import { ParkingSlot } from "../../interface/interface";
import BookingService from "../../services/bookingService";
import ParkingSlotItem from "./ParkingSlotItem";

interface ParkingSlotListProps {
	parkingSlots: ParkingSlot[];
	handleBookParkingSlot: (slotId: string) => void;
}

export default function ParkingSlotList({
	parkingSlots,
	handleBookParkingSlot,
}: ParkingSlotListProps) {
	console.log("Liste des places de parking :", parkingSlots);
	if (!parkingSlots.length) {
		return <div>Aucune place de parking disponible pour cette date.</div>;
	}

	return (
		<div>
			<h2 className="text-xl font-semibold mb-2">Places disponibles :</h2>
			<ul className="list-disc ml-5">
				{parkingSlots.map((slot) => (
					<ParkingSlotItem
						key={slot.id}
						parkingSlot={slot}
						handleBookParkingSlot={handleBookParkingSlot}
					/>
				))}
			</ul>
		</div>
	);
}
