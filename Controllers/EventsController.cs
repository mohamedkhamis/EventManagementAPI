using EventManagementAPI.Data;
using EventManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Controllers;

public class EventController(AppDbContext context) : Controller
{
    // GET: Event
    public async Task<IActionResult> Index(string searchQuery, string location, DateTime? startDate, DateTime? endDate, EventStatus? status)
    {
        var events = context.Events.AsQueryable();
        var eventsList = await events.ToListAsync();

        // Search filters
        if (!string.IsNullOrEmpty(searchQuery))
        {
            events = events.Where(e => e.Title.Contains(searchQuery) || e.Description.Contains(searchQuery));
        }

        if (!string.IsNullOrEmpty(location))
        {
            events = events.Where(e => e.Location.Contains(location));
        }

        if (startDate.HasValue)
        {
            events = events.Where(e => e.Date >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            events = events.Where(e => e.Date <= endDate.Value.Date);
        }

        if (status.HasValue)
        {
            events = events.Where(e => e.Status == status);
        }

        var model = await events.ToListAsync();
        return View(model);
    }

    // GET: Event/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Event/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event @event)
    {
        if (ModelState.IsValid)
        {
            context.Add(@event);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(@event);
    }

    // GET: Event/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var @event = await context.Events.FindAsync(id);
        if (@event == null) return NotFound();

        return View(@event);
    }

    // POST: Event/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event @event)
    {
        if (id != @event.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(@event);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(@event.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(@event);
    }

    // GET: Event/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var @event = await context.Events.FirstOrDefaultAsync(e => e.Id == id);
        if (@event == null) return NotFound();

        return View(@event);
    }

    // POST: Event/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @event = await context.Events.FindAsync(id);
        if (@event != null)
        {
            context.Events.Remove(@event);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Event/Details/5
    public IActionResult Details(int id)
    {
        var eventItem = context.Events.Find(id);
    
        if (eventItem == null)
        {
            return NotFound();
        }
    
        return View(eventItem);
    }
    private bool EventExists(int id) => context.Events.Any(e => e.Id == id);
}