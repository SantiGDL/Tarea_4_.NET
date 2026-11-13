// Obtengo los elementos del formulario
const formularioLogin = document.getElementById('loginForm');
const botonLogin = document.getElementById('loginButton');
const mensajeVerificacion = document.getElementById('verificationMessage');

// Creo la conexión con el Hub
var conexion = new signalR.HubConnectionBuilder()
    .withUrl("/loginHub")
    .build();

// Recibo la confirmación del servidor y redirijo al usuario
conexion.on("VerificacionOk", function (usuario) {
    console.info("Me invocaron desde el servidor, usuario: " + usuario);

    window.location.href = "/PaginaBienvenida";
});

// Recibo la URL de verificación que genera el servidor
conexion.on("UrlVerificacion", function(url) {
    console.log("URL para verificar el usuario: " + url);

    // Le asigno la URL al enlace y lo muestro en la página
    const enlace = document.getElementById("linkVerificar");
    enlace.href = url;

    document.getElementById("enlaceVerificacion").style.display = "block";
});

// Inicio la conexión con SignalR
conexion.start()
    .then(function () {
        console.info("Conexión con SignalR establecida");

        botonLogin.disabled = false;
        botonLogin.textContent = "Ingresar";
    })
    .catch(function (error) {
        console.error("Error al conectar con SignalR:", error.toString());

        mensajeVerificacion.style.display = 'block';
        mensajeVerificacion.textContent = "Error al conectar con el servidor.";
    });

// Capturo el envío del formulario
formularioLogin.addEventListener('submit', function (evento) {

    // Evito que se recargue la página
    evento.preventDefault();

    // Antes de enviar los datos compruebo que siga conectado
    if (conexion.state !== signalR.HubConnectionState.Connected) {
        mensajeVerificacion.style.display = 'block';
        mensajeVerificacion.textContent = "No hay conexión con el servidor.";
        return;
    }

    // Bloqueo el botón mientras proceso el login
    botonLogin.disabled = true;
    botonLogin.textContent = "Procesando...";

    // Obtengo los datos ingresados
    var correo = document.getElementById("email").value;
    var contrasena = document.getElementById("password").value;

    console.info("Envío mensaje al Hub");

    // Llamo a Login implementado en LoginHub.cs
    conexion.invoke("Login", correo, contrasena)
        .then(function () {
            mensajeVerificacion.style.display = 'block';
            mensajeVerificacion.textContent =
                "Esperando verificación del usuario...";

            botonLogin.textContent = "Esperando verificación...";
        })
        .catch(function (error) {
            console.error("Error al invocar Login:", error.toString());

            mensajeVerificacion.style.display = 'block';
            mensajeVerificacion.textContent =
                "Ocurrió un error al procesar el inicio de sesión.";

            botonLogin.disabled = false;
            botonLogin.textContent = "Ingresar";
        });
});