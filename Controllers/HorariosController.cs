using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;
using WebInstituto.Models.Enum;
using WebInstituto.Repositorios;
using WebInstituto.ViewModels.Horarios;


namespace WebInstituto.Controllers
{
    public class HorariosController : Controller
    {
        private readonly RepoHorario repoHorarios;
        private readonly RepoAsignaturas repoAsignaturas; // Iniciar el repositorio de asignaturas

        public HorariosController()
        {
            DBSqlite db = new DBSqlite();
            this.repoHorarios = new RepoHorario(db);
            this.repoAsignaturas = new RepoAsignaturas(db); // Iniciar el repositorio de asignaturas
        }

        public ActionResult Horario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            // Obtener los horarios que coincidan con el id de la asignatura
            IList<Horario> horarios = repoHorarios.GetAll().Where(h => h.AsignaturaId == id).ToList();

            // Obtener la asignatura correspondiente
            Asignatura asignatura = repoAsignaturas.GetById(id);


            // Almacenar horarios al ViewModel
            IList<HorarioViewModel> horariosViewModel = horarios.Select(h => new HorarioViewModel
            {
                SubjectHorario = h.Asignatura,
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
        public ActionResult FormularioCrearHorario(int asignaturaId)
        {
            IList<Asignatura> asignaturas = repoAsignaturas.GetAll();
            Asignatura asignaturaEncontrada = new Asignatura();
            foreach (Asignatura a in asignaturas)
            {
                if (a.Id == asignaturaId)
                {
                    asignaturaEncontrada = a;
                }
            }
            HorarioViewModel viewModel = new HorarioViewModel()
            {
                SubjectHorario = asignaturaEncontrada,
                DiasSemanales = new List<SelectListItem>
                {
                    new SelectListItem{Value=DiasSemana.Lunes.ToString(), Text="Lunes"},
                    new SelectListItem{Value=DiasSemana.Martes.ToString(), Text="Martes" },
                    new SelectListItem{Value=DiasSemana.Miercoles.ToString(), Text="Miércoles" },
                    new SelectListItem{Value=DiasSemana.Jueves.ToString(), Text="Jueves" },
                    new SelectListItem{Value=DiasSemana.Viernes.ToString(), Text="Viernes" }
                }
            };
            return View(viewModel);
        }
    }
}
