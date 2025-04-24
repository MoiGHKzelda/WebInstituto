namespace Proyecto.Services
{
    public static class SeguridadService
    {
        // Genera un hash de la contraseña usando BCrypt
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        // Verifica si la contraseña ingresada coincide con el hash almacenado
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
