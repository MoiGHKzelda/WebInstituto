using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;
using WebInstituto.Models.Enum;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Asignaturas;



namespace WebInstituto.Controllers
{
    public class AsignaturasController : Controller
    {
        public readonly RepoAsignaturas repoAsignaturas; // Iniciar el repositorio de asignaturas
        private readonly SessionService sessionService;
        public AsignaturasController(SessionService sesion)
        {
            DBSqlite db = new DBSqlite();
            this.repoAsignaturas = new RepoAsignaturas(db);
            sessionService = sesion;
        }
        
        [HttpPost]
        public ActionResult CrearAsignatura(CrearAsignaturasViewModel asignaturaCreada, int Curso)
        {
            try
            {
                // El valor del curso seleccionado viene directamente como un parámetro 'Curso'
                int cursoSeleccionado = Curso;

                // Crear la asignatura con el valor del curso
                repoAsignaturas.CrearAsignatura(new Asignatura(
                    asignaturaCreada.Name, cursoSeleccionado
                ));

                return View("~/Views/Asignaturas/VistaLogeado.cshtml");
            }
            catch (Exception ex)
            {
                // Manejo de error genérico
                ModelState.AddModelError(string.Empty, "Hubo un error al crear la asignatura.");
                return View("~/Views/Asignaturas/CrearAsignatura.cshtml", asignaturaCreada);
            }
        }

        public ActionResult FormAsignaturaCrear()
        {
            CrearAsignaturasViewModel viewModel = new CrearAsignaturasViewModel()
            {
                Cursos = new List<SelectListItem>
                {
                    new SelectListItem{Value=DiasSemana.Lunes.ToString(), Text="1º"},
                    new SelectListItem{Value=DiasSemana.Lunes.ToString(), Text="2º"},
                    new SelectListItem{Value=DiasSemana.Lunes.ToString(), Text="3º"},
                    new SelectListItem{Value=DiasSemana.Lunes.ToString(), Text="4º"},
                }
            };

            return View("~/Views/Asignaturas/CrearAsignatura.cshtml", viewModel);
        }

        public ActionResult VistaAsignaturas()
        {
            AsignaturasViewModel viewModel = new AsignaturasViewModel()
            {
                Asignaturas = repoAsignaturas.GetAll(),
                EsProfesor = sessionService.EsProfesor()
            };

            return View("~/Views/Asignaturas/VistaAsignaturas.cshtml", viewModel);
        }
    }
}
