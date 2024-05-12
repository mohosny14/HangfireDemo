using Hangfire;
using HangfireDemo.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString("DefultConnection");

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(ConnectionString)
    );

builder.Services.AddHangfire(x => x.UseSqlServerStorage(ConnectionString));
builder.Services.AddHangfireServer();


#region Example with Hangfire from old project i used it with OrderService Notification
//var serviceProvider = builder.Services.BuildServiceProvider();
//var recurringJobManager = serviceProvider.GetService<IRecurringJobManager>();
//var orderService = serviceProvider.GetService<IOrderService>();
//var notificationService = serviceProvider.GetService<INotificationService>();

//recurringJobManager.AddOrUpdate(
//    "CheckIncompleteOrdersDesignForNotification", // Unique identifier for the job
//    () => orderService.CheckIncompleteOrdersDesignForNotification(), // Method to execute
//    Cron.Daily // Cron expression defining the schedule (daily)
//);
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/hangfireDashboard");
app.Run();
