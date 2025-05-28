interface BookingListProps {
	bookings: any[];
}

export default function BookingList({ bookings }: BookingListProps) {
	if (!bookings.length) {
		return <div>Aucune réservation pour cette date.</div>;
	}

	return (
		<div>
			<h2 className="text-xl font-semibold mb-2">Places disponibles :</h2>
			<ul className="list-disc ml-5">
				{bookings.map((booking, index) => (
					<li key={index}>
						Place: {booking.spotNumber} – Status: {booking.status}
					</li>
				))}
			</ul>
		</div>
	);
}
