using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
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
            this.sessionService =sessionService;
        }

        public ActionResult Login()
        {            
            //HttpContext.Session.Clear();
            LoginViewModel viewModel = new LoginViewModel();
            return View("~/Views/Personas/Login.cshtml", viewModel);
        }

        public ActionResult LoginSave(LoginViewModel persona)
        {
            Persona personaObj=repoPersonas.GetByEmail(persona.Mail);
            if (ComprobarUsuario(personaObj, persona.ContrasenyaHash))
            {
                sessionService.Login(personaObj);
                return RedirectToAction("VistaAsignaturas", "Asignaturas");
            }
            persona.TieneErrores = true;
            return View("~/Views/Personas/Login.cshtml", persona);
        }



        public ActionResult Registro()
        {
            RegistroViewModel viewModel = new RegistroViewModel();
            return View("~/Views/Personas/Registro.cshtml", viewModel);
        }
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

        private bool ComprobarUsuario(Persona persona, string password)
        {
            if (persona == null)
            {
                return false;
            }
            return SeguridadService.VerifyPassword(password, persona.ContrasenyaHash);
            
        }
    }

}
