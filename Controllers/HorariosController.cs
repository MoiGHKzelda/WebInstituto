using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;
using WebInstituto.Models.Enum;
using WebInstituto.Repositorios;
using WebInstituto.Services;
using WebInstituto.ViewModels.Horarios;

namespace WebInstituto.Controllers
{
    public class HorariosController : Controller
    {
        private readonly RepoHorario repoHorarios;
        private readonly RepoAsignaturas repoAsignaturas;
        private readonly SessionService sessionService;

        public HorariosController(SessionService sessionService)
        {
            DBSqlite db = new DBSqlite();
            this.repoHorarios = new RepoHorario(db);
            this.repoAsignaturas = new RepoAsignaturas(db);
            this.sessionService = sessionService;
        }

        // Muestra los horarios asociados a una asignatura específica.
        public ActionResult Horario(int id)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = true;

            // Obtiene todos los horarios filtrados por AsignaturaId.
            IList<Horario> horarios = repoHorarios.GetAll().Where(h => h.AsignaturaId == id).ToList();
            Asignatura asignatura = repoAsignaturas.GetById(id);

            // Mapea los horarios a un ViewModel para enviarlos a la vista.
            IList<HorarioViewModel> horariosViewModel = horarios.Select(h => new HorarioViewModel
            {
                Asignatura = h.Asignatura,
                Day = h.Day,
                Start = h.Start,
                End = h.End
            }).ToList();

            HorariosViewModel viewModel = new HorariosViewModel()
            {
                Horarios = horariosViewModel,
                AsignaturaId = asignatura.Id,
                PageTitle = $"Vista Horarios de {asignatura.Name} de {asignatura.Course}º"
            };

            ViewData["Title"] = "Vista Horarios";
            return View(viewModel);
        }

        // Renderiza el formulario para crear o editar un horario.
        public ActionResult FormularioCrearHorario(int asignaturaId, int horarioId = 0)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            ViewBag.EstaLogeado = true;

            Asignatura asignatura = repoAsignaturas.GetById(asignaturaId);
            if (asignatura == null)
                return BadRequest("Asignatura no encontrada.");

            // Prepara opciones para horas y días.
            FormularioHorarioViewModel viewModel = new FormularioHorarioViewModel()
            {
                HorarioLectivo = new List<SelectListItem>
                {
                    new SelectListItem { Value = "08:00", Text = "08:00" },
                    new SelectListItem { Value = "08:30", Text = "08:30" },
                    new SelectListItem { Value = "09:00", Text = "09:00" },
                    new SelectListItem { Value = "09:30", Text = "09:30" },
                    new SelectListItem { Value = "10:00", Text = "10:00" },
                    new SelectListItem { Value = "10:30", Text = "10:30" },
                    new SelectListItem { Value = "11:00", Text = "11:00" },
                    new SelectListItem { Value = "11:30", Text = "11:30" },
                    new SelectListItem { Value = "12:00", Text = "12:00" },
                    new SelectListItem { Value = "12:30", Text = "12:30" },
                    new SelectListItem { Value = "13:00", Text = "13:00" },
                    new SelectListItem { Value = "13:30", Text = "13:30" },
                    new SelectListItem { Value = "14:00", Text = "14:00" },
                    new SelectListItem { Value = "14:30", Text = "14:30" },
                    new SelectListItem { Value = "15:00", Text = "15:00" }
                },
                DiasSemanales = Enum.GetValues(typeof(DiasSemana))
                    .Cast<DiasSemana>()
                    .Select(d => new SelectListItem
                    {
                        Text = d.ToString(),
                        Value = ((int)d).ToString()
                    }).ToList(),
                Asignatura = asignatura,
                Id = horarioId
            };

            // Si es edición, carga datos del horario existente.
            if (horarioId != 0)
            {
                Horario horarioExistente = repoHorarios.GetById(horarioId);
                if (horarioExistente == null)
                    return NotFound("Horario no encontrado.");

                viewModel.Start = horarioExistente.Start;
                viewModel.End = horarioExistente.End;
                viewModel.Day = horarioExistente.Day;
            }

            return View(viewModel);
        }

        // Guarda o actualiza un horario en base al ID recibido.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearHorario(HorarioViewModel horarioViewModel)
        {
            if (!sessionService.EstaLogeado())
                return (ActionResult)sessionService.NoLogin();

            if (horarioViewModel.Id != 0)
            {
                // Edición de horario existente.
                Horario horarioExistente = repoHorarios.GetById(horarioViewModel.Id);
                if (horarioExistente == null)
                    return NotFound("Horario no encontrado.");

                horarioExistente.Start = horarioViewModel.Start;
                horarioExistente.End = horarioViewModel.End;
                horarioExistente.Day = horarioViewModel.Day;
                repoHorarios.ActualizarHorario(horarioExistente);
            }
            else
            {
                // Creación de nuevo horario.
                Horario nuevoHorario = new Horario
                {
                    Start = horarioViewModel.Start,
                    End = horarioViewModel.End,
                    Day = horarioViewModel.Day,
                    AsignaturaId = horarioViewModel.AsignaturaId
                };
                repoHorarios.CrearHorario(nuevoHorario);
            }

            return RedirectToAction("VistaAsignatura", "Asignaturas", new { id = horarioViewModel.AsignaturaId });
        }

        // Elimina un horario según su ID.
        [HttpPost]
        public IActionResult EliminarHorario(int asignaturaId, int horarioId)
        {
            bool eliminado = repoHorarios.EliminarHorario(horarioId);

            if (eliminado)
            {
                return Ok(new { success = true, asignaturaId = asignaturaId });
            }
            else
            {
                return NotFound(new { success = false, message = "No se encontró el horario o no se pudo eliminar." });
            }
        }
    }
}
