using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;

namespace WebInstituto.ViewModels.Horarios
{
    public class FormularioHorarioViewModel
    {
        public int Id { get; set; }
        public Asignatura Asignatura { get; set; }
        public int AsignaturaId { get; set; }
        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        public List<SelectListItem> DiasSemanales { get; set; }
        public List<SelectListItem> HorarioLectivo { get; set; }
    }
}
