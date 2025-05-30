import "./App.css";
import { Routes, Route } from "react-router-dom";

import BookingPage from "./pages/BookingPage";
import LoginPage from "./pages/LoginPage";
import SignUpPage from "./pages/SignUpPage";
import HistoryPage from "./pages/HistoryPage";
import WithHeaderLayout from "./components/ui/with-header-layout";
import WithoutHeaderLayout from "./components/ui/without-header-layout";
import ProtectedRoute from "./ProtectedRoute";
import UnauthorizedPage from "./pages/UnauthorizedPage";
import { UserRole } from "./interface/interface";
import StatisTicsPage from "./pages/StatisticsPage";

function App() {
	return (
		<Routes>
			<Route element={<WithoutHeaderLayout />}>
				<Route path="/" element={<LoginPage />} />
				<Route path="/sign-up" element={<SignUpPage />} />
			</Route>

			<Route element={<WithHeaderLayout />}>
				<Route element={<ProtectedRoute allowedRoles={[UserRole.MANAGER]} />}>
					<Route path="/statistics" element={<StatisTicsPage />} />
				</Route>
				<Route path="/unauthorized" element={<UnauthorizedPage />} />
				<Route path="/Booking" element={<BookingPage />} />
				<Route path="/History" element={<HistoryPage />} />
			</Route>
		</Routes>
	);
}

export default App;
