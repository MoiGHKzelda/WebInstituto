using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;

namespace WebInstituto.ViewModels.Horarios
{
    public class HorarioViewModel
    {
        public Asignatura SubjectHorario {  get; set; }
        public int Day {  get; set; }
        public string Start {  get; set; }
        public string End { get; set; }
        public List<SelectListItem> DiasSemanales { get; set; }
    }
}
