import "./App.css";
import { Routes, Route } from "react-router-dom";

import BookingPage from "./pages/BookingPage";
import LoginPage from "./pages/LoginPage";
import SignUpPage from "./pages/SignUpPage";
import HistoryPage from "./pages/HistoryPage";
import WithHeaderLayout from "./components/ui/with-header-layout";
import WithoutHeaderLayout from "./components/ui/without-header-layout";

function App() {
	return (
		<Routes>
			<Route element={<WithoutHeaderLayout />}>
				<Route path="/" element={<LoginPage />} />
				<Route path="/sign-up" element={<SignUpPage />} />
			</Route>

			<Route element={<WithHeaderLayout />}>
				<Route path="/Booking" element={<BookingPage />} />
				<Route path="/History" element={<HistoryPage />} />
			</Route>
		</Routes>
	);
}

export default App;
