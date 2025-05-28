import { useState, useEffect } from "react";
import BookingService from "../../services/bookingService";

export default function Booking() {
	const [bookings, setBookings] = useState([]);
	useEffect(() => {
		BookingService.getParkingStatePerDate(new Date(Date.now()))
			.then((response) => {
				console.log(response);
				setBookings([]);
			})
			.catch((error) => {
				console.error("There was an error fetching the reservations!", error);
			});
	}, []);
	return <div>bookings</div>;
}
