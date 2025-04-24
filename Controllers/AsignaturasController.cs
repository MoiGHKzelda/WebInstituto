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
        // Repositorios y servicios utilizados en el controlador
        private readonly RepoAsignaturas repoAsignaturas;
        private readonly SessionService sessionService;
        private readonly RepoPersonas repoPersonas;
        private readonly RepoMatricular repoMatricula;

        // Constructor: Inicializa los repositorios con la conexión y el servicio de sesión
        public AsignaturasController(SessionService sesion)
        {
            DBSqlite db = new DBSqlite();
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.repoPersonas = new RepoPersonas(db);
            this.repoMatricula = new RepoMatricular(db);
            sessionService = sesion;
        }

        // Crear o editar una asignatura según el Id recibido
        [HttpPost]
        public ActionResult CrearEditarAsignatura(CrearEditarAsignaturasViewModel model)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            try
            {
                if (model.Asignatura.Id == 0)
                {
                    var asignatura = new Asignatura(model.Asignatura.Name, model.Asignatura.Course);
                    repoAsignaturas.CrearAsignatura(asignatura);
                }
                else
                {
                    repoAsignaturas.ActualizarAsignatura(model.Asignatura);
                }

                return RedirectToAction("VistaAsignaturas");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Hubo un error al guardar la asignatura.");
                return View(model);
            }
        }

        // Elimina una asignatura por ID, responde con Ok o NotFound
        [HttpPost]
        public IActionResult EliminarAsignatura(int id)
        {
            bool eliminado = repoAsignaturas.EliminarAsignatura(id);
            return eliminado ? Ok() : NotFound("No se encontró la asignatura.");
        }

        // Muestra el formulario de creación o edición de asignatura
        public ActionResult FormAsignatura(int idAsignatura)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            Asignatura asigEditar = idAsignatura == 0 ? null : repoAsignaturas.GetById(idAsignatura);
            bool editar = idAsignatura != 0;

            var viewModel = new FormAsignaturaViewModel()
            {
                Asignatura = asigEditar,
                Cursos = new List<SelectListItem>
                {
                    new SelectListItem{Value=((int)Cursos.Primero).ToString(), Text="1º"},
                    new SelectListItem{Value=((int)Cursos.Segundo).ToString(), Text="2º"},
                    new SelectListItem{Value=((int)Cursos.Tercero).ToString(), Text="3º"},
                    new SelectListItem{Value=((int)Cursos.Cuarto).ToString(), Text="4º"}
                },
                Editar = editar
            };

            return View("~/Views/Asignaturas/CrearEditarAsignatura.cshtml", viewModel);
        }

        // Lista todas las asignaturas y define si el usuario es profesor
        public ActionResult VistaAsignaturas()
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            var viewModel = new AsignaturasViewModel()
            {
                Asignaturas = repoAsignaturas.GetAll(),
                EsProfesor = sessionService.EsProfesor()
            };

            return View("~/Views/Asignaturas/VistaAsignaturas.cshtml", viewModel);
        }

        // Muestra detalles de una asignatura e indica si el usuario está matriculado
        public ActionResult VistaAsignatura(int id)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            var asignatura = repoAsignaturas.GetById(id);

            //  Ordena los horarios
            if (asignatura.Horarios != null)
            {
                asignatura.Horarios = asignatura.Horarios
                    .OrderBy(h => h.Day)
                    .ThenBy(h => h.Start)
                    .ToList();
            }

            var personaMail = sessionService.GetMailPersona();
            var persona = repoPersonas.GetByEmail(personaMail);

            bool yaMatriculado = persona != null && repoMatricula.ExisteMatriculacion(new AsignaturaPersona(persona.Id.Value, asignatura.Id));

            var viewModel = new AsignaturaViewModel()
            {
                Asignatura = asignatura,
                EsProfesor = sessionService.EsProfesor(),
                PersonaMail = personaMail,
                EstaMatriculado = yaMatriculado
            };

            ViewData["Title"] = "Vista Horarios";

            return View("~/Views/Asignaturas/VistaAsignatura.cshtml", viewModel);
        }

        // Cambia el estado de asignar o liberar un profesor a una asignatura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstadoImpartir([FromBody] AccionesAsignaturaDatos datos)
        {
            try
            {
                var asignatura = repoAsignaturas.GetById(datos.AsignaturaId);
                if (asignatura == null)
                    return NotFound(new { message = "Asignatura no encontrada." });

                string personaMail = sessionService.GetMailPersona();

                if (asignatura.Profesor != null && asignatura.Profesor.Mail != personaMail)
                    return BadRequest(new { message = "Esta asignatura ya está siendo impartida por otro profesor." });

                var persona = repoPersonas.GetByEmail(personaMail);
                if (persona == null)
                    return BadRequest(new { message = "No se encontró la persona con el correo especificado." });

                asignatura.Profesor = datos.Activar ? persona : null;
                repoAsignaturas.ActualizarAsignatura(asignatura);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar el estado de impartir.", error = ex.Message });
            }
        }

        // Cambia el estado de matrícula del usuario en la asignatura
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstadoMatricular([FromBody] AccionesAsignaturaDatos datos)
        {
            try
            {
                var asignatura = repoAsignaturas.GetById(datos.AsignaturaId);
                if (asignatura == null) return NotFound(new { message = "Asignatura no encontrada." });

                var personaMail = sessionService.GetMailPersona();
                var persona = repoPersonas.GetByEmail(personaMail);
                if (persona == null) return BadRequest(new { message = "Persona no encontrada." });

                var personaMatriculada = new AsignaturaPersona(persona.Id.Value, asignatura.Id);

                if (datos.Activar && !repoMatricula.ExisteMatriculacion(personaMatriculada))
                    repoMatricula.CrearMatriculacion(personaMatriculada);
                else if (!datos.Activar && repoMatricula.ExisteMatriculacion(personaMatriculada))
                    repoMatricula.EliminarMatriculacion(personaMatriculada);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar la matrícula.", error = ex.Message });
            }
        }
    }
}
