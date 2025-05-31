import { useState } from "react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import AuthService from "@/services/authService";
import { useNavigate } from "react-router-dom";
import { UserRole } from "@/interface/interface";
import { toast } from "sonner";

export function SignUpForm({
	className,
	...props
}: React.ComponentProps<"div">) {
	const [formData, setFormData] = useState({
		firstName: "",
		lastName: "",
		email: "",
		password: "",
	});

	const navigate = useNavigate();

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		setFormData({ ...formData, [e.target.id]: e.target.value });
	};

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();

		const dataToSend = {
			role: UserRole.USER,
			...formData,
		};

		try {
			const response = AuthService.register(dataToSend);

			if (!response) throw new Error("Failed to sign up");

			toast.success("Compte créé avec succès");

			navigate("/");
		} catch (error) {
			toast.error(
				`Une erreur s'est produite lors de votre création de compte. Si cela se reproduit, merci de vous rapprocher de l'administrateur de l'application.`,
				{
					duration: 2000,
				}
			);
		}
	};

	return (
		<div className={cn("flex flex-col gap-6", className)} {...props}>
			<Card>
				<CardHeader>
					<CardTitle>Create your account</CardTitle>
					<CardDescription>
						Fill in your details below to create an account.
					</CardDescription>
				</CardHeader>
				<CardContent>
					<form onSubmit={handleSubmit}>
						<div className="flex flex-col gap-6">
							<div className="grid gap-3">
								<Label htmlFor="firstName">First Name</Label>
								<Input
									id="firstName"
									type="text"
									value={formData.firstName}
									onChange={handleChange}
									required
								/>
							</div>
							<div className="grid gap-3">
								<Label htmlFor="lastName">Last Name</Label>
								<Input
									id="lastName"
									type="text"
									value={formData.lastName}
									onChange={handleChange}
									required
								/>
							</div>
							<div className="grid gap-3">
								<Label htmlFor="email">Email</Label>
								<Input
									id="email"
									type="email"
									value={formData.email}
									onChange={handleChange}
									required
								/>
							</div>
							<div className="grid gap-3">
								<Label htmlFor="password">Password</Label>
								<Input
									id="password"
									type="password"
									value={formData.password}
									onChange={handleChange}
									required
								/>
							</div>
							<div className="flex flex-col gap-3">
								<Button type="submit" className="w-full">
									Sign Up
								</Button>
							</div>
						</div>
					</form>
				</CardContent>
			</Card>
		</div>
	);
}
