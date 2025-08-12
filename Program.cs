using EventManagementAPI.Data;
using EventManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Event}/{action=Index}/{id?}");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Events.Any())
    {
        context.Events.AddRange(
            new Event
            {
                Title = "Team Conference",
                Date = DateTime.Now.AddDays(5),
                Location = "Toronto",
                Description = "Annual dev conference",
                Status = EventStatus.Upcoming
            },
            new Event
            {
                Title = "Doctor Appointment",
                Date = DateTime.Now.AddHours(48),
                Location = "Montreal Clinic",
                Description = "Yearly checkup",
                Status = EventStatus.Attending
            }
        );
        context.SaveChanges();
    }
}

app.Run();