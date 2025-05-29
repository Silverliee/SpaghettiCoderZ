import "./App.css";
import { Routes, Route } from "react-router-dom";

import BookingPage from "./pages/BookingPage";
import LoginPage from "./pages/LoginPage";
import SignUpPage from "./pages/SignUpPage";

function App() {
	return (
		<Routes>
			<Route path="/" element={<LoginPage />} />
			<Route path="/sign-up" element={<SignUpPage />} />
			<Route path="/Booking" element={<BookingPage />} />
		</Routes>
	);
}

export default App;
