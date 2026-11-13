using Tarea_4_Razor_Pages_SignalR.Modelos;
using Microsoft.AspNetCore.SignalR;

namespace Tarea_4_Razor_Pages_SignalR.Hubs
{
    public class LoginHub : Hub
    {
        private readonly ILogger<LoginHub> _logger;

        public LoginHub(ILogger<LoginHub> logger)
        {
            _logger = logger;
        }

        
        public async Task Login(string correo, string contrasena)
        {
            _logger.LogInformation(
                "SignalR identificacion del usuario: " + Context.ConnectionId
            );

            Usuario usr = new Usuario(correo, contrasena);

            if (usr.EsUsuarioValido())
            {
                if (usr.NecesitaVerificacion())
                {
                    // Simulamos el envío del correo
                    string usrId = Context.ConnectionId;

                    string urlVerificacion =
                        $"https://localhost:44372/verificar/usuario/{usrId}";

                    // Mostramos la URL en la consola del servidor (Rider)
                    _logger.LogInformation(
                        "**** Copiar la siguiente URL para probar"
                    );

                    _logger.LogInformation(urlVerificacion);

                    // Enviamos la URL al navegador mediante SignalR
                    await Clients.Caller.SendAsync(
                        "UrlVerificacion",
                        urlVerificacion
                    );
                }
            }
        }


        public async Task EnviarVerificacionOk()
        {
            await Clients.Caller.SendAsync("VerificacionOk", "");
        }
    }
}