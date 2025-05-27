import React from "react";
import axios from "axios";

export default function Reservation() {
	const [reservations, setReservations] = React.useState([]);
	React.useEffect(() => {
		axios
			.get(`${process.env.REACT_APP_API_BACK_URL}/Reservation`)
			.then((response) => {
				console.log(response.data);
				setReservations(response.data.reservations);
			})
			.catch((error) => {
				console.error("There was an error fetching the reservations!", error);
			});
	}, []);
	return <div>reservation</div>;
}
