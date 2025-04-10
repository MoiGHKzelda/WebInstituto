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
        public ActionResult CrearAsignatura(CrearAsignaturasViewModel asignaturaCreada, string Curso)
        {
            try
            {
                // El valor del curso seleccionado viene directamente como un parámetro 'Curso'
                int cursoSeleccionado = int.Parse(Curso);

                // Crear la asignatura con el valor del curso
                repoAsignaturas.CrearAsignatura(new Asignatura(
                    asignaturaCreada.Name, cursoSeleccionado
                ));

                return RedirectToAction("VistaAsignaturas");
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
                    new SelectListItem{Value=((int)Cursos.Primero).ToString(), Text="1º"},
                    new SelectListItem{Value=((int)Cursos.Segundo).ToString(), Text="2º"},
                    new SelectListItem{Value=((int)Cursos.Tercero).ToString(), Text="3º"},
                    new SelectListItem{Value=((int)Cursos.Cuarto).ToString(), Text="4º"}
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
        public ActionResult VistaAsignatura(int id)
        {
            // Obtener la asignatura correspondiente
            Asignatura asignatura = repoAsignaturas.GetById(id);


            // viemModel final que se enviará a la vista
            AsignaturaViewModel viewModel = new AsignaturaViewModel()
            {
                Asignatura = asignatura,
                EsProfesor = sessionService.EsProfesor(),
                PersonaMail = sessionService.GetMailPersona()
            };

            ViewData["Title"] = "Vista Horarios";

            return View("~/Views/Asignaturas/VistaAsignatura.cshtml", viewModel);
        }

        //Testeo
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
                // Obtener la asignatura a través del ID
                Asignatura asignatura = repoAsignaturas.GetById(datos.AsignaturaId);
                if (asignatura == null)
                {
                    return NotFound(new { message = "Asignatura no encontrada." });
                }

                // Obtener el correo de la persona logueada desde el servicio de sesión
                string personaMail = sessionService.GetMailPersona();

                // Buscar la persona (el estudiante) por su correo en el repositorio de personas
                Persona persona = repoPersonas.GetByEmail(personaMail);  // Asegúrate de tener un método en tu repositorio que busque por mail.
                if (persona == null)
                {
                    return BadRequest(new { message = "No se encontró la persona con el correo especificado." });
                }

                // Crear el objeto de AsignaturaPersona con los datos proporcionados
                AsignaturaPersona personaMatriculada = new AsignaturaPersona(persona.Id.Value, asignatura.Id);

                // Si activar es true, matriculamos al estudiante
                if (datos.Activar)
                {
                    // Comprobamos si la persona ya está matriculada
                    bool yaMatriculado = repoMatricula.ExisteMatriculacion(personaMatriculada);
                    if (yaMatriculado)
                    {
                        return BadRequest(new { message = "El estudiante ya está matriculado en esta asignatura." });
                    }

                    // Crear la matrícula
                    repoMatricula.crearMatriculacion(personaMatriculada);
                }
                else
                {
                    // Si activar es false, desmatriculamos al estudiante
                    bool matriculacionExiste = repoMatricula.ExisteMatriculacion(personaMatriculada);
                    if (!matriculacionExiste)
                    {
                        return BadRequest(new { message = "El estudiante no está matriculado en esta asignatura." });
                    }

                    // Eliminar la matrícula
                    repoMatricula.eliminarMatriculacion(personaMatriculada);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al cambiar el estado de matriculación.", error = ex.Message });
            }
        }
    }
}

