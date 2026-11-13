namespace Tarea_4_Razor_Pages_SignalR.Modelos
{
    public class Usuario
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Usuario(string email, string pass)
        {
            this.Email = email;
            this.Password = pass;
        }
        
        // Siempre devuelven True porque no implemento una BD o una verificación real
        public bool EsUsuarioValido()
        {
            
            return true;
        }

        public bool NecesitaVerificacion()
        {
            return true;
        }
    }
}