using Microsoft.AspNetCore.Mvc;
using Proyecto.Services;
using WebInstituto.Models;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Personas;

namespace WebInstituto.Controllers
{
    public class LoginController : Controller
    {
        private readonly RepoPersonas repoPersonas;
        private readonly RepoAsignaturas repoAsignaturas;
        private readonly SessionService sessionService;

        public LoginController(SessionService sessionService)
        {
            DBSqlite db = new DBSqlite();
            this.repoPersonas = new RepoPersonas(db);
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.sessionService = sessionService;
        }

        // Muestra la vista de login y asegura que la sesión esté cerrada.
        public ActionResult Login()
        {
            sessionService.Logout();
            LoginViewModel viewModel = new LoginViewModel();
            return View("~/Views/Personas/Login.cshtml", viewModel);
        }

        // Valida credenciales y gestiona el inicio de sesión.
        public ActionResult LoginSave(LoginViewModel persona)
        {
            Persona personaObj = repoPersonas.GetByEmail(persona.Mail);
            if (ComprobarUsuario(personaObj, persona.ContrasenyaHash))
            {
                sessionService.Login(personaObj);
                return RedirectToAction("VistaAsignaturas", "Asignaturas");
            }

            persona.TieneErrores = true;
            return View("~/Views/Personas/Login.cshtml", persona);
        }

        // Muestra la vista de registro.
        public ActionResult Registro()
        {
            RegistroViewModel viewModel = new RegistroViewModel();
            return View("~/Views/Personas/Registro.cshtml", viewModel);
        }

        // Guarda un nuevo usuario en la base de datos.
        public ActionResult RegistroSave(RegistroViewModel personaRegistrada)
        {
            repoPersonas.CrearPersona(new Persona(
                personaRegistrada.Name,
                personaRegistrada.LastName,
                personaRegistrada.Teacher,
                personaRegistrada.EmergencyPhone,
                personaRegistrada.Mail,
                personaRegistrada.ContrasenyaHash
            ));

            LoginViewModel viewModel = new LoginViewModel();
            return View("~/Views/Personas/Login.cshtml", viewModel);
        }

        // Comprueba si las credenciales son correctas.
        private bool ComprobarUsuario(Persona persona, string password)
        {
            if (persona == null)
                return false;

            return SeguridadService.VerifyPassword(password, persona.ContrasenyaHash);
        }
    }
}
