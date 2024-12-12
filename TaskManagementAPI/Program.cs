using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagementAPI.Data;
using TaskManagementAPI.Mapping;
using TaskManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskDbContext>(options => options.UseInMemoryDatabase("TaskDb"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ITaskService, TaskService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var logger = new LoggerConfiguration()
    .WriteTo.File("Logs/CollegeAdmissions_Logs.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

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

app.Run();
