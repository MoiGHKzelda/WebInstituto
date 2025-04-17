using WebInstituto.Models;

namespace WebInstituto.ViewModels.Asignaturas
{
    public class AsignaturaViewModel
    {
        public Asignatura Asignatura { get; set; }
        public bool EsProfesor { get; set; }
        public string PersonaMail { get; set; }
        public bool EstaMatriculado { get; set; }

    }
}
