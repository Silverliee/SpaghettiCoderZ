// components/ui/DateRangePicker.tsx
import { Calendar } from "@/components/ui/calendar";
import {
	Popover,
	PopoverContent,
	PopoverTrigger,
} from "@/components/ui/popover";
import { Button } from "@/components/ui/button";
import { format } from "date-fns";
import { CalendarIcon } from "lucide-react";
import { DateRange } from "react-day-picker";

type Props = {
	value?: DateRange;
	onChange: (range: DateRange | undefined) => void;
};

export function DateRangePicker({ value, onChange }: Props) {
	return (
		<Popover>
			<PopoverTrigger asChild>
				<Button variant="outline" className="w-full justify-start text-left">
					<CalendarIcon className="mr-2 h-4 w-4" />
					{value?.from ? (
						value.to ? (
							`${format(value.from, "dd/MM/yyyy")} → ${format(
								value.to,
								"dd/MM/yyyy"
							)}`
						) : (
							format(value.from, "dd/MM/yyyy")
						)
					) : (
						<span>Choisir une plage de dates</span>
					)}
				</Button>
			</PopoverTrigger>
			<PopoverContent className="w-auto p-0" align="start">
				<Calendar
					mode="range"
					selected={value}
					onSelect={onChange}
					initialFocus
				/>
			</PopoverContent>
		</Popover>
	);
}
