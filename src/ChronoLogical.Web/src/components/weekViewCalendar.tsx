import { useRef, useState } from "react";
import { DayPilot, DayPilotCalendar } from "@daypilot/daypilot-lite-react";
import { Box, Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from "@mui/material";
import TimeEntryApi from "../service/timeEntryApi.ts";

interface CalendarEvent {
  id: string;
  text: string;
  start: string;
  end: string;
}

export default function WeekViewCalendar() {
  const calendarRef = useRef<DayPilotCalendar>(null);
  const [events, setEvents] = useState<CalendarEvent[]>([]);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [description, setDescription] = useState("");
  const [duration, setDuration] = useState<number>(1);
  const [selectedStart, setSelectedStart] = useState<string | null>(null);

  // Handle time range selection
  const onTimeRangeSelected = async (args: any) => {
    setSelectedStart(args.start.toString());
    setDialogOpen(true);
  };

  // Handle event move (drag and drop)
  const onEventMove = async (args: any) => {
    setEvents((prev) =>
      prev.map((ev) =>
        ev.id === args.e.data.id
          ? { ...ev, start: args.newStart.toString(), end: args.newEnd.toString() }
          : ev
      )
    );
    // Optionally update backend here
  };

  // Add new event
  const handleAddEntry = async () => {
    if (!selectedStart) return;
    const start = DayPilot.Date.parse(selectedStart, "yyyy-MM-ddTHH:mm:ss");
    const end = start.addHours(duration);
    const newEvent: CalendarEvent = {
      id: DayPilot.guid(),
      text: description,
      start: start.toString(),
      end: end.toString(),
    };
    try {
      await TimeEntryApi.addTimeEntry({
        date: start.toString("yyyy-MM-dd"),
        description,
        duration,
      });
      setEvents([...events, newEvent]);
      setDialogOpen(false);
      setDescription("");
      setDuration(1);
    } catch {
      alert("Failed to add time entry.");
    }
  };

  return (
    <Box>
      <DayPilotCalendar
        ref={calendarRef}
        viewType="Week"
        events={events}
        durationBarVisible={false}
        onTimeRangeSelected={onTimeRangeSelected}
        onEventMove={onEventMove}
        heightSpec="Full"
      />
      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
        <DialogTitle>Add Time Entry</DialogTitle>
        <DialogContent>
          <TextField
            label="Description"
            value={description}
            onChange={e => setDescription(e.target.value)}
            fullWidth
            margin="dense"
          />
          <TextField
            label="Duration (hours)"
            type="number"
            value={duration}
            onChange={e => setDuration(Number(e.target.value))}
            fullWidth
            margin="dense"
            inputProps={{ min: 0.25, step: 0.25 }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDialogOpen(false)}>Cancel</Button>
          <Button onClick={handleAddEntry} variant="contained">Add</Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}