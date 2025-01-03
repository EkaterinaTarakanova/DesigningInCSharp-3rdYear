using SchedulePlanner.Domain.Entities;
using SchedulePlanner.Domain.EventAttributes;
using SchedulePlanner.Domain.Interfaces;

namespace SchedulePlanner.Application.CalendarEvents.EventRules.Rules;

public class SingleOnlyEventRule(ICalendarEventRepository calendarEventRepository) : IEventRule
{
    public string FailMessage => "Пересечение обязательного календарного события с другим";

    public async Task<bool> CheckAsync(CalendarEvent calendarEvent)
    {
        var userId = calendarEvent.UserId;
        var start = calendarEvent.StartDate;
        var end = calendarEvent.EndDate;
        
        if (!calendarEvent.AttributeData.TryGetAttribute<SingleOnlyEventAttribute>(out var singleOnlyEventAttribute) 
            || !singleOnlyEventAttribute!.IsSingleOnly)
        {
            return !await calendarEventRepository.AnySingleOnlyAsync(userId, start, end);
        }

        return !await calendarEventRepository.AnyAsync(userId, start, end);
    }
}