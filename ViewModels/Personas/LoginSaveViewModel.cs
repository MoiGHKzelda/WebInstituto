using WebInstituto.Models;
using WebInstituto.Repositorios;

namespace WebInstituto.ViewModels.Personas
{
    public class LoginSaveViewModel
    {
        public Persona Usuario {  get; set; }
        public IList<Asignatura> Asignaturas { get; set; }
    }
}
