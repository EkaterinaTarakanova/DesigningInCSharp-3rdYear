using Microsoft.EntityFrameworkCore;
using SchedulePlanner.Application.CalendarEvents;
using SchedulePlanner.Domain.Entities;
using SchedulePlanner.Domain.EventAttributes;
using SchedulePlanner.Domain.EventAttributes.Attributes;
using SchedulePlanner.Infrastructure.Common;

namespace SchedulePlanner.Infrastructure.Repositories;

public class CalendarEventRepository : BaseRepository, ICalendarEventRepository
{
    private readonly AppDbContext context;

    public CalendarEventRepository(AppDbContext context) : base(context) => this.context = context;

    public async Task<List<CalendarEvent>> GetByUserIdAsync(Guid userId, DateTime start, DateTime end)
    {
        return await context.CalendarEvents
            .Where(e => e.UserId == userId && e.EndDate >= start && e.StartDate <= end)
            .ToListAsync();
    }
    
    public async Task<List<CalendarEvent>> GetAllAsync(DateTime start, DateTime end)
    {
        return await context.CalendarEvents
            .Where(e => e.EndDate >= start && e.StartDate <= end)
            .ToListAsync();
    }

    public async Task<List<CalendarEvent>> GetPublicByUserIdAsync(Guid userId, DateTime start, DateTime end)
    {
        var calendarEvents = await GetByUserIdAsync(userId, start, end);

        return calendarEvents
            .Where(e => e.IsPublic())
            .ToList();
    }

    public async Task<CalendarEvent?> GetByIdAsync(Guid id)
    {
        return await context.CalendarEvents.FindAsync(id);
    }

    public async Task<List<CalendarEvent>> GetByIdsAsync(IReadOnlyCollection<Guid> ids)
    {
        return await context.CalendarEvents.Where(e => ids.Contains(e.Id)).ToListAsync();
    }

    public void Create(CalendarEvent newEvent)
    {
        context.CalendarEvents.Add(newEvent);
    }
    
    public void Delete(CalendarEvent calendarEvent)
    {
        context.CalendarEvents.Remove(calendarEvent);
    }

    public async Task<bool> AnyAsync(Guid userId, DateTime start, DateTime end)
    {
        return await context.CalendarEvents
            .AnyAsync(e => e.UserId == userId && e.EndDate < end && e.StartDate > start);
    }

    public async Task<bool> AnySingleOnlyAsync(Guid userId, DateTime start, DateTime end)
    {
        var calendarEvents = await GetByUserIdAsync(userId, start, end);

        return calendarEvents.Any(
            e => e.AttributeData.HasActiveAttribute<MandatoryEventAttribute>());
    }

    public async Task<bool> AnyWithLocationAsync(string location, DateTime start, DateTime end)
    {
        var calendarEvents = await GetAllAsync(start, end);

        return calendarEvents.Any(e =>
            e.AttributeData.HasActiveAttribute<DependsOnLocationEventAttribute>(attr =>
                attr.Location!.Equals(location)));
    }
}