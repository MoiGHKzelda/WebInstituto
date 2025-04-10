using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models.Enum;
using WebInstituto.Models;

namespace WebInstituto.ViewModels.Horarios
{
    public class FormularioHorarioViewModel : Controller
    {
        public Asignatura Asignatura { get; set; }
        public int AsignaturaId { get; set; }
        public List<SelectListItem> DiasSemanales { get; set; }
        public List<SelectListItem> HorarioLectivo { get; set; }
    }
}
