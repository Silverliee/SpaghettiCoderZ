import * as React from "react";
import { Calendar } from "@/components/ui/calendar";
import { Card, CardContent } from "@/components/ui/card";

export function DatePicker() {
	const [date, setDate] = React.useState<Date | undefined>();

	const handleDateChange = (selected: Date | undefined) => {
		console.log("Date sélectionnée:", selected);
		setDate(selected);
	};

	return (
		<Card className="w-fit p-4">
			<CardContent className="p-0">
				<Calendar
					mode="single"
					selected={date}
					onSelect={handleDateChange}
					className="rounded-md border shadow"
				/>
				{date && (
					<p className="mt-4 text-center text-sm text-muted-foreground">
						Date choisie: {date.toDateString()}
					</p>
				)}
			</CardContent>
		</Card>
	);
}
