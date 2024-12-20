using SchedulePlanner.Domain.Common;
using SchedulePlanner.Domain.EventAttributes;

namespace SchedulePlanner.Domain.Entities;

public class CalendarEvent : Entity<Guid>
{
    public Guid UserId { get; }
    
    public DateTime StartDate { get; }
    
    public DateTime EndDate { get; }
    
    private readonly Dictionary<Type, IEventAttribute> attributes = new();
    public IReadOnlyDictionary<Type, IEventAttribute> Attributes => attributes;

    public CalendarEvent(Guid userId, DateTime startDate, DateTime endDate) : base(Guid.NewGuid())
    {
        UserId = userId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public CalendarEvent AddAttribute<T>(T newAttribute) where T : IEventAttribute
    {
        var key = typeof(T);

        if (!attributes.TryAdd(key, newAttribute))
        {
            throw new InvalidOperationException($"Attribute of type {key.Name} is already added.");
        }

        return this;
    }

    public void UpdateAttribute<T>(T attribute) where T : IEventAttribute
    {
        var key = typeof(T);
        attributes[key] = attribute;
    }

    public CalendarEvent RemoveAttribute<T>() where T : IEventAttribute
    {
        var key = typeof(T);

        if (!attributes.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Attribute of type {key.Name} does not exist.");
        }

        attributes.Remove(key);
        return this;
    }

    public T? GetAttribute<T>() where T : IEventAttribute
    {
        var key = typeof(T);

        return attributes.TryGetValue(key, out var attribute) 
            ? (T?)attribute 
            : default;
    }

    public T GetRequiredAttribute<T>() where T : IEventAttribute
    {
        var key = typeof(T);

        return (T)attributes[key];
    }

    public bool TryGetAttribute<T>(out T? attribute) where T : IEventAttribute
    {
        var key = typeof(T);
        var success = attributes.TryGetValue(key, out var value);
        attribute = success ? (T?)value : default;
        return success;
    }

    public bool HasAttribute(Type attributeType) => attributes.ContainsKey(attributeType);

    public bool HasAttribute<T>() where T : IEventAttribute 
        => attributes.ContainsKey(typeof(T));
}