using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;
using WebInstituto.Models.Enum;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Asignaturas;
using WebInstituto.ViewModels.Horarios;



namespace WebInstituto.Controllers
{
    public class AsignaturasController : Controller
    {
        public readonly RepoAsignaturas repoAsignaturas; // Iniciar el repositorio de asignaturas
        private readonly SessionService sessionService;
        public readonly RepoPersonas repoPersonas;
        public readonly RepoMatricular repoMatricula;
        public AsignaturasController(SessionService sesion)
        {
            DBSqlite db = new DBSqlite();
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.repoPersonas = new RepoPersonas(db);
            this.repoMatricula = new RepoMatricular(db);
            sessionService = sesion;
        }

        [HttpPost]
        public ActionResult CrearEditarAsignatura(CrearEditarAsignaturasViewModel model)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();
            try
            {
                // Si el Id es 0, significa que estamos creando una nueva asignatura
                if (model.Asignatura.Id == 0)
                {
                    // Crear una nueva asignatura
                    var asignatura = new Asignatura(model.Asignatura.Name, model.Asignatura.Course);
                    repoAsignaturas.CrearAsignatura(asignatura);
                }
                else
                {
                    // Si el Id no es 0, significa que estamos actualizando una asignatura existente
                    repoAsignaturas.ActualizarAsignatura(model.Asignatura);
                }

                return RedirectToAction("VistaAsignaturas"); // Redirigir a la vista de asignaturas
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Hubo un error al guardar la asignatura.");
                return View(model); // Devolver el mismo modelo con los errores
            }
        }
        [HttpPost]
        public IActionResult EliminarAsignatura(int id)
        {
            bool eliminado = repoAsignaturas.EliminarAsignatura(id);

            if (eliminado)
            {
                return Ok();
            }
            else
            {
                return NotFound("No se encontró la asignatura.");
            }
        }
        public ActionResult FormAsignatura(int idAsignatura)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();
            Console.WriteLine("Entra al controlador");
            Asignatura asigEditar;
            bool editar;
            if (idAsignatura == 0)
            {
                asigEditar = null;
                editar = false;
            }
            else
            {
                asigEditar = repoAsignaturas.GetById(idAsignatura);
                editar = true;
            }
            FormAsignaturaViewModel viewModel = new FormAsignaturaViewModel()
            {
                Asignatura = asigEditar,
                Cursos = new List<SelectListItem>
                {
                    new SelectListItem{Value=((int)Cursos.Primero).ToString(), Text="1º"},
                    new SelectListItem{Value=((int)Cursos.Segundo).ToString(), Text="2º"},
                    new SelectListItem{Value=((int)Cursos.Tercero).ToString(), Text="3º"},
                    new SelectListItem{Value=((int)Cursos.Cuarto).ToString(), Text="4º"}
                },
                Editar=editar
            };

            return View("~/Views/Asignaturas/CrearEditarAsignatura.cshtml", viewModel);
        }

        public ActionResult VistaAsignaturas()
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();
            AsignaturasViewModel viewModel = new AsignaturasViewModel()
            {
                Asignaturas = repoAsignaturas.GetAll(),
                EsProfesor = sessionService.EsProfesor()
            };

            return View("~/Views/Asignaturas/VistaAsignaturas.cshtml", viewModel);
        }
        public ActionResult VistaAsignatura(int id)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();
            // Obtener la asignatura correspondiente
            Asignatura asignatura = repoAsignaturas.GetById(id);

            // Obtener el correo de la persona logueada
            string personaMail = sessionService.GetMailPersona();
            Persona persona = repoPersonas.GetByEmail(personaMail);

            // Verificar si ya está matriculado
            bool yaMatriculado = false;
            if (persona != null)
            {
                var personaMatriculada = new AsignaturaPersona(persona.Id.Value, asignatura.Id);
                yaMatriculado = repoMatricula.ExisteMatriculacion(personaMatriculada);
            }

            // ViemModel final que se enviará a la vista
            AsignaturaViewModel viewModel = new AsignaturaViewModel()
            {
                Asignatura = asignatura,
                EsProfesor = sessionService.EsProfesor(),
                PersonaMail = personaMail,
                EstaMatriculado = yaMatriculado  // Enviar el estado de matriculación
            };

            ViewData["Title"] = "Vista Horarios";

            return View("~/Views/Asignaturas/VistaAsignatura.cshtml", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstadoImpartir([FromBody] AccionesAsignaturaDatos datos)
        {
            try
            {
                Asignatura asignatura = repoAsignaturas.GetById(datos.AsignaturaId);
                if (asignatura == null)
                {
                    return NotFound(new { message = "Asignatura no encontrada." });
                }

                // Obtener el correo de la persona logueada desde el servicio de sesión
                string personaMail = sessionService.GetMailPersona();

                // Validar si la persona logueada ya es el profesor asignado
                if (asignatura.Profesor != null && asignatura.Profesor.Mail != personaMail)
                {
                    return BadRequest(new { message = "Esta asignatura ya está siendo impartida por otro profesor." });
                }

                // Buscar la persona por su correo en el repositorio de personas
                Persona persona = repoPersonas.GetByEmail(personaMail);  // Asegúrate de tener un método en tu repositorio que busque por mail.
                if (persona == null)
                {
                    return BadRequest(new { message = "No se encontró la persona con el correo especificado." });
                }

                // Si se activa, asignamos al profesor
                if (datos.Activar)
                {
                    // Asignamos la persona existente como profesor
                    asignatura.Profesor = persona;
                }
                else
                {
                    // Si se desactiva, eliminamos al profesor
                    asignatura.Profesor = null;
                }

                // Actualizamos la asignatura con los cambios
                repoAsignaturas.ActualizarAsignatura(asignatura);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar el estado de impartir.", error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstadoMatricular([FromBody] AccionesAsignaturaDatos datos)
        {
            try
            {
                var asignatura = repoAsignaturas.GetById(datos.AsignaturaId);
                if (asignatura == null) return NotFound(new { message = "Asignatura no encontrada." });

                string personaMail = sessionService.GetMailPersona();
                var persona = repoPersonas.GetByEmail(personaMail);
                if (persona == null) return BadRequest(new { message = "Persona no encontrada." });

                var personaMatriculada = new AsignaturaPersona(persona.Id.Value, asignatura.Id);

                if (datos.Activar)
                {
                    if (!repoMatricula.ExisteMatriculacion(personaMatriculada))
                        repoMatricula.crearMatriculacion(personaMatriculada);
                }
                else
                {
                    if (repoMatricula.ExisteMatriculacion(personaMatriculada))
                        repoMatricula.eliminarMatriculacion(personaMatriculada);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar la matrícula.", error = ex.Message });
            }
        }
    }
}

