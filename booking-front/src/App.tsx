import "./App.css";
import { Routes, Route } from "react-router-dom";

import BookingPage from "./pages/BookingPage";
import LoginPage from "./pages/LoginPage";

function App() {
	return (
		<Routes>
			<Route path="/login" element={<LoginPage />} />
			<Route path="/" element={<BookingPage />} />
		</Routes>
	);
}

export default App;
