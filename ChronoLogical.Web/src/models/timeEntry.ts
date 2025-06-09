export default class TimeEntry {
  date: string;
  description: string;
  duration: number;
  
  constructor({ date, description, duration }: { date: string; description: string; duration: number }) {
    this.date = date; // string, e.g. '2025-06-09'
    this.description = description; // string
    this.duration = duration; // number (hours)
  }
}