using SchedulePlanner.Domain.Entities;

namespace SchedulePlanner.Domain.Interfaces;

public interface ICalendarEventRepository
{
    public void AddEvent(CalendarEvent newEvent);
    public List<CalendarEvent> GetAllEvents();
    public void UpdateEvent(CalendarEvent updatedEvent);
    public void DeleteEvent(string id);
    public CalendarEvent[] GetEvents(DateTime start, DateTime end);

    public bool Any(DateTime start, DateTime end);
}