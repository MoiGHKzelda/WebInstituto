using Microsoft.AspNetCore.Mvc;
using WebInstituto.Models;
using WebInstituto.Repositorios;
using WebInstituto.ViewMoldels.Personas;

namespace WebInstituto.Controllers
{
    public class PersonasController : Controller
    {

        private readonly RepoPersonas repoPersonas; // Iniciar el repositorio de personas
        public PersonasController() 
        {
            DBSqlite db = new DBSqlite();
            this.repoPersonas = new RepoPersonas(db);
        }
        public IActionResult Index()
        {
            // Obtener todas las asignaturas
            IList<Persona> personas = repoPersonas.GetAll();
            IList<PersonaViewModel> personasViewModel= new List<PersonaViewModel>();
            // Almacenar horarios al ViewModel
            foreach (Persona persona in personas)
            {
                PersonaViewModel personaViewModel = new PersonaViewModel()
                {
                    Name = persona.Name,
                    LastName = persona.LastName
                };
                personasViewModel.Add(personaViewModel);
            }

            // viemModel final que se enviará a la vista
            ProfesoresViewModel viewModel = new ProfesoresViewModel()
            {
                Profesores=personasViewModel,
                PageTitle = "Vista Persona"
            };
            ViewData["Title"] = "Vista Persona";
            return View(viewModel);
        }
    }
}
