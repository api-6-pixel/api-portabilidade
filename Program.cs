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

app.MapGet("/getUser/{idUser}", async (int idUser, IKersysApi api, CancellationToken cancellationToken) =>
{
    await api.CreateNewAccessToken(new KersysParameters(DateTime.MaxValue), cancellationToken);
    return Results.Ok(await api.GetUserDataAsync(idUser.ToString(), cancellationToken));
})
.WithOpenApi();

app.MapGet("/getUserExpirado/{idUser}", async (int idUser, IKersysApi api, CancellationToken cancellationToken) =>
{
    await api.CreateNewAccessToken(new KersysParameters(DateTime.Now.AddDays(-1)), cancellationToken);
    return Results.Ok(await api.GetUserDataAsync(idUser.ToString(), cancellationToken));
})
.WithOpenApi();

app.Run();

