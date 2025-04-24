using Microsoft.AspNetCore.Mvc;
using WebInstituto.Models;

namespace WebInstituto.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Inyección de dependencias para acceder a la HttpContext
        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Guardar el correo electrónico y el tipo de usuario (si es profesor o no) en la sesión
        public void Login(Persona persona)
        {
            _httpContextAccessor.HttpContext.Session.SetString("email", persona.Mail);
            _httpContextAccessor.HttpContext.Session.SetInt32("isTeacher", persona.Teacher.Value);
        }

        // Cerrar la sesión y limpiar los datos de la sesión
        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        // Verificar si el usuario está logueado
        public bool EstaLogeado()
        {
            var email = _httpContextAccessor.HttpContext.Session.GetString("email");
            return !string.IsNullOrEmpty(email);
        }
        //Comprueba si es profesor
        public bool EsProfesor()
        {
            int? teacher = _httpContextAccessor.HttpContext.Session.GetInt32("isTeacher");
            if (teacher.HasValue)
            {
                return teacher.Value != 0;
            }
            return false;
        }
        // Obtiene el mail de una persona
        public string GetMailPersona()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("email");
        }
        // Redirigir si no está logueado
        public IActionResult NoLogin()
        {
            return new RedirectToActionResult("Login", "Login", null);
        }
    }
}
