import TimeEntry from '../models/timeEntry.ts';

const API_BASE_URL = 'https://localhost:7196/api/timeentries'; // Replace with your API URL

class TimeEntryApi {
  static async addTimeEntry(entry : TimeEntry): Promise<TimeEntry> {
    const response = await fetch(API_BASE_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(entry),
    });
    if (!response.ok) throw new Error('Failed to add time entry');
    return response.json();
  }

  // Optionally, add more methods (get, update, delete) here
}

export default TimeEntryApi;