using Microsoft.AspNetCore.Mvc;
using WebInstituto.Models;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Personas;
using WebInstituto.ViewMoldels.Personas;

namespace WebInstituto.Controllers
{
    public class PersonasController : Controller
    {

        private readonly RepoPersonas repoPersonas; // Iniciar el repositorio de personas
        private readonly RepoAsignaturas repoAsignaturas;
        private readonly SessionService sessionService;
        public PersonasController(SessionService sessionService) 
        {
            DBSqlite db = new DBSqlite();
            this.repoPersonas = new RepoPersonas(db);
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.sessionService = sessionService;
        }
        public IActionResult ListadoAlumno(int asignaturaId)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();
            Asignatura asignatura = repoAsignaturas.GetById(asignaturaId);
            ListadoAlumnoViewModel viewModel = new ListadoAlumnoViewModel()
            {
                Asignatura = asignatura
            };
            return View("~/Views/Personas/ListadoAlumno.cshtml", viewModel);
        }
    }
}
