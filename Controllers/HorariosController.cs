using System.Collections.Generic;
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

        public ActionResult Horario(int id)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            // Obtener los horarios que coincidan con el id de la asignatura
            IList<Horario> horarios = repoHorarios.GetAll().Where(h => h.AsignaturaId == id).ToList();

            // Obtener la asignatura correspondiente
            Asignatura asignatura = repoAsignaturas.GetById(id);

            // Almacenar horarios al ViewModel
            IList<HorarioViewModel> horariosViewModel = horarios.Select(h => new HorarioViewModel
            {
                Asignatura = h.Asignatura,
                Day = h.Day,
                Start = h.Start,
                End = h.End
            }).ToList();

            // viemModel final que se enviará a la vista
            HorariosViewModel viewModel = new HorariosViewModel()
            {
                Horarios = horariosViewModel,
                AsignaturaId = asignatura.Id,
                PageTitle = $"Vista Horarios de {asignatura.Name} de {asignatura.Course}º" // Ahora se muestra el nombre de la asignatura
            };

            ViewData["Title"] = "Vista Horarios";
            return View(viewModel);
        }
        public ActionResult FormularioCrearHorario(int asignaturaId, int horarioId = 0)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            ViewBag.EstaLogeado = sessionService.EstaLogeado();

            // Obtén la asignatura mediante el id
            Asignatura asignatura = repoAsignaturas.GetById(asignaturaId);
            if (asignatura == null)
            {
                return BadRequest("Asignatura no encontrada.");
            }

            // Crea el view model y carga los dropdowns
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
                // Atributo para distinguir creación o edición
                Id = horarioId
            };

            // Si se recibe un horarioId, buscamos el horario existente y llenamos el view model
            if (horarioId != 0)
            {
                Horario horarioExistente = repoHorarios.GetById(horarioId);
                if (horarioExistente == null)
                {
                    return NotFound("Horario no encontrado.");
                }
                viewModel.Start = horarioExistente.Start;
                viewModel.End = horarioExistente.End;
                viewModel.Day = horarioExistente.Day;
            }

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearHorario(HorarioViewModel horarioViewModel)
        {
            if (!sessionService.EstaLogeado())
            {
                return (ActionResult)sessionService.NoLogin();
            }
            //Modo edición
            if (horarioViewModel.Id != 0)
            {
                Horario horarioExistente = repoHorarios.GetById(horarioViewModel.Id);
                if (horarioExistente == null)
                {
                    return NotFound("Horario no encontrado.");
                }
                horarioExistente.Start = horarioViewModel.Start;
                horarioExistente.End = horarioViewModel.End;
                horarioExistente.Day = horarioViewModel.Day;
                repoHorarios.ActualizarHorario(horarioExistente);
            }
            else // Modo creación
            {
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

    }
}
