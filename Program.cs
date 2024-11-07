using CommonConnector.Contracts;
using DiscordConnector;
using DiscordRichPresenceConnector;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
//builder.Services.RegisterDiscordConnector();
builder.Services.RegisterDiscordRichPresenceConnector();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


app.MapGet("/listening", async (IPublicationConnector connector, IConfiguration configuration, [FromQuery(Name = "m")] string? message) => {
    var token = configuration["Discord:Token"];

    _ = await connector.Connect(token!);
    _ = await connector.UpdateStatus(message ?? "Ma musique", PublicationTargets.All);

}).WithName("SetListening")
.WithOpenApi();

app.MapGet("/discord/bot/status", async (IPublicationConnector connector, IConfiguration configuration, [FromQuery(Name = "m")] string? message) => {
    var token = configuration["Discord:Token"];

    _ = await connector.Connect(token!);
    _ = await connector.UpdateStatus(message ?? "Ma musique", PublicationTargets.All);

}).WithName("SetDiscordBotStatus")
.WithOpenApi();



//app.MapGet("/discord/status", async (IPublicationConnector connector, IConfiguration configuration, [FromQuery(Name ="m")]string? message) => {
//    var token = configuration["Discord:Token"];

//    _ = await connector.Connect(token!);
//    _ = await connector.UpdateStatus(message ?? "Hello, World!");
//    //await Task.Delay(500);
//    //await connector.Disconnect();

//}).WithName("SetStatusMessage")
//.WithOpenApi();


app.Run();

