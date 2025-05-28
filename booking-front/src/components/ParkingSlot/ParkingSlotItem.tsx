import { ParkingSlot } from "../../interface/interface";

export default function ParkingSlotItem({
	parkingSlot,
	handleBookParkingSlot,
}: {
	parkingSlot: ParkingSlot;
	handleBookParkingSlot: (slotId: string) => void;
}) {
	return (
		<div key={parkingSlot.id}>
			Place: {parkingSlot.id} {parkingSlot.hasCharger ? "– Electrique " : ""}
			<button
				className="ml-2 bg-blue-500 text-white px-3 py-1 rounded"
				onClick={() => handleBookParkingSlot(parkingSlot.id)}
			>
				Réserver
			</button>
		</div>
	);
}
