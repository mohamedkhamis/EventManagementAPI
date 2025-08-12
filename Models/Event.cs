namespace EventManagementAPI.Models;

public enum EventStatus
{
    Upcoming,
    Attending,
    Maybe,
    Declined
}

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EventStatus Status { get; set; } = EventStatus.Upcoming;
}