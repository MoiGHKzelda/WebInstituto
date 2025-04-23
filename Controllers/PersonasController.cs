using Microsoft.AspNetCore.Mvc;
using WebInstituto.Models;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Personas;

namespace WebInstituto.Controllers
{
    public class PersonasController : Controller
    {
        private readonly RepoPersonas repoPersonas; // Repositorio para manejar personas.
        private readonly RepoAsignaturas repoAsignaturas; // Repositorio para manejar asignaturas.
        private readonly SessionService sessionService; // Servicio para controlar la sesión.

        public PersonasController(SessionService sessionService)
        {
            DBSqlite db = new DBSqlite();
            this.repoPersonas = new RepoPersonas(db);
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.sessionService = sessionService;
        }

        // Muestra el listado de alumnos asociados a una asignatura específica.
        public IActionResult ListadoAlumno(int asignaturaId)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }

            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            // Obtiene la asignatura por ID.
            Asignatura asignatura = repoAsignaturas.GetById(asignaturaId);

            // Crea el ViewModel para pasar datos a la vista.
            ListadoAlumnoViewModel viewModel = new ListadoAlumnoViewModel()
            {
                Asignatura = asignatura
            };

            return View("~/Views/Personas/ListadoAlumno.cshtml", viewModel);
        }
    }
}
