using SchedulePlanner.Domain.Entities;

namespace SchedulePlanner.Domain.EventRules;

public interface IEventRule
{
    public int Priority { get; }

    public bool Check(CalendarEvent calendarEvent);
}