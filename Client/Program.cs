using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PixelLib;
using PixelLib.DI;
using PixelLib.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injecao de dependencia
builder.Services.RegisterKersysApi((http) =>
{
    http.BaseAddress = new Uri("http://localhost:8080/api/portabilidade");
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/webhook-user", ([FromBody] UserEncrpyted userEncrypted, IKersysApi api) =>
{
    Console.WriteLine(api.DecryptUser(userEncrypted.dados, userEncrypted.aesIv, userEncrypted.aesKey));
})
.WithOpenApi();

app.MapPost("/create-key", async (IKersysApi api, CancellationToken cancellationToken) =>
{
    await api.CreateNewAccessToken(cancellationToken);
    return Results.Ok();
})
.WithOpenApi();

app.MapGet("/getUser/{userId}", async (int userId, IKersysApi api, CancellationToken cancellationToken) =>
{
    return Results.Ok(await api.GetUserDataAsync(userId.ToString(), new KersysParameters(DateTime.Now.AddDays(120)), cancellationToken));
})
.WithOpenApi();


app.Run();

public record AuthorizePortability(bool Accepted, string AuthToken);
public record UserEncrpyted(String dados, String aesKey, String aesIv);
