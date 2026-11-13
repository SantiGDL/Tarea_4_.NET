using Tarea_4_Razor_Pages_SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configuración HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Registro del Hub SignalR
app.MapHub<LoginHub>("/loginHub");

// Ruta que simula la verificación del correo electrónico
app.MapGet("/verificar/usuario/{userId}",
    async (string userId,
        ILogger<LoginHub> logger,
        IHubContext<LoginHub> hubContext) =>
    {
        logger.LogInformation(
            $"Se notificará al cliente con id {userId}"
        );

        // Notifico al navegador que inició el login
        await hubContext.Clients.Client(userId)
            .SendAsync("VerificacionOk", userId);

        return Results.Ok("Usuario verificado correctamente");
    });

app.Run();