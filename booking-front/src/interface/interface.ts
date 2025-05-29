export enum BookingStatus {
	BOOKED = 0,
	CANCELLED = 1,
	COMPLETED = 2,
}

export interface Booking {
	id: string;
	parkingSlotId: string;
	date: Date;
	//userId: string;
	status: BookingStatus;
}
export interface ParkingSlot {
	id: string;
	hasCharger: boolean;
	//isAvailable: boolean;
}
// export interface User {
// 	lastName: string;
// 	firstName: string;
// 	email: string;
// 	role: string;
// }

export interface LoginPayload {
	email: string;
	password: string;
}

export interface RegisterPayload {
	firstName: string;
	lastName: string;
	email: string;
	password: string;
	role: number;
}

export interface RegisterResponse {
	firstName: string;
	lastName: string;
	email: string;
	password: string;
	role: number;
}

export interface AuthResponse {
	token: string;
	user: {
		id: number;
		name: string;
		email: string;
	};
}

export {};
