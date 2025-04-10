using Microsoft.AspNetCore.Mvc;
using WebInstituto.Models;
using WebInstituto.Repositorios;
using WebInstituto.ViewModels.Personas;
using WebInstituto.ViewMoldels.Personas;

namespace WebInstituto.Controllers
{
    public class PersonasController : Controller
    {

        private readonly RepoPersonas repoPersonas; // Iniciar el repositorio de personas
        private readonly RepoAsignaturas repoAsignaturas;
        public PersonasController() 
        {
            DBSqlite db = new DBSqlite();
            this.repoPersonas = new RepoPersonas(db);
            this.repoAsignaturas = new RepoAsignaturas(db);
        }
        public IActionResult ListadoAlumno(int asignaturaId)
        {
            Asignatura asignatura = repoAsignaturas.GetById(asignaturaId);
            ListadoAlumnoViewModel viewModel = new ListadoAlumnoViewModel()
            {
                Asignatura = asignatura
            };
            return View("~/Views/Personas/ListadoAlumno.cshtml", viewModel);
        }
    }
}
