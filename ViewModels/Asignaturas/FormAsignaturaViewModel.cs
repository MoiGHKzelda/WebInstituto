using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;

namespace WebInstituto.ViewModels.Asignaturas
{
    public class FormAsignaturaViewModel
    {
        public Asignatura Asignatura { get; set; }

        public List<SelectListItem> Cursos { get; set; }

        public bool Editar { get; set; }

        public FormAsignaturaViewModel()
        {
            Asignatura = new Asignatura();
            Cursos = new List<SelectListItem>();
        }
    }
}
