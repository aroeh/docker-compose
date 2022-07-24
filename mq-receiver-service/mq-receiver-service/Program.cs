using mq_receiver_service.DataAccess;
using mq_receiver_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<WeatherReceiver>();
builder.Services.AddSingleton<IMongoWeather, MongoWeather>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => 
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .WithMethods(new string[] { "GET", "POST", "PUT" })
);

app.UseAuthorization();

app.MapControllers();

app.Run();
