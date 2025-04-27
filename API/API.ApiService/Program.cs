using Microsoft.AspNetCore.Mvc;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.Models;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// === Rejestracja zale¿noœci ===
builder.Services.AddScoped<ReservationRepo>();
builder.Services.AddScoped<ReservationService>();

// === Rejestracja MySQL ===
builder.Services.AddSingleton<MySqlDataSource>(_ =>
    new MySqlDataSource("Server=localhost;Database=san;User ID=root;Password=;Port=3306;")
);

// === CORS dla po³¹czenia z frontem ===
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(); // U¿yj CORS przed jakimikolwiek endpointami

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// === Minimal API ===
var rezerwacje = app.MapGroup("/api/rezerwacje");

rezerwacje.MapPost("/rezerwuj", async (
    [FromBody] RezerwacjaRequest request,
    ReservationService reservationService) =>
{
    var sukces = await reservationService.ZarezerwujSaleAsync(
        request.NumerSali,
        request.Login,
        request.DataOd,
        request.DataDo
    );

    return sukces
        ? Results.Ok(new { message = "Sala zarezerwowana pomyœlnie!" })
        : Results.Conflict(new { message = "Sala jest ju¿ zajêta w tym terminie." });
});

rezerwacje.MapDelete("/usun/{idRezerwacji}", async (
    int idRezerwacji,
    ReservationService reservationService) =>
{
    var sukces = await reservationService.UsunRezerwacjeAsync(idRezerwacji);
    return sukces
        ? Results.Ok(new { message = "Rezerwacja usuniêta." })
        : Results.NotFound(new { message = "Nie znaleziono rezerwacji." });
});

rezerwacje.MapPut("/edytuj/{idRezerwacji}", async (
    int idRezerwacji,
    [FromBody] EdytujRezerwacjeRequest request,
    ReservationService reservationService) =>
{
    var sukces = await reservationService.EdytujRezerwacjeAsync(idRezerwacji, request.NowaDataOd, request.NowaDataDo);
    return sukces
        ? Results.Ok(new { message = "Rezerwacja zaktualizowana." })
        : Results.Conflict(new { message = "Nie uda³o siê zaktualizowaæ rezerwacji." });
});

app.Run();
