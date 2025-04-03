using WebInstituto.Models;

namespace WebInstituto.ViewModels.Asignaturas
{
    public class AsignaturasViewModel
    {
        public IList<Asignatura> Asignaturas { get; set; }
        public bool EsProfesor {  get; set; }
    }
}
