using Proyecto.Services;
using WebInstituto.Models;

namespace WebInstituto.Repositorios
{
    public class RepoPersonas
    {
        // Contexto de base de datos
        private readonly DBSqlite Db;

        // Constructor que recibe la conexión
        public RepoPersonas(DBSqlite db)
        {
            this.Db = db;
        }

        // Obtiene todas las personas registradas
        public IList<Persona> GetAll()
        {
            return Db.Persona.ToList();
        }

        // Crea una nueva persona si no existe un email igual
        public Persona CrearPersona(Persona persona)
        {
            if (!Db.Persona.Any(a => a.Mail == persona.Mail))
            {
                // Encripta la contraseña antes de guardar
                persona.ContrasenyaHash = SeguridadService.HashPassword(persona.ContrasenyaHash);
                Persona personaNew = Db.Persona.Add(persona).Entity;
                Db.SaveChanges();
                return persona;
            }
            else
            {
                Console.WriteLine("Usuario ya registrado");
                return null;
            }
        }

        // Busca una persona por su email
        public Persona GetByEmail(string mail)
        {
            return Db.Persona.FirstOrDefault(p => p.Mail == mail);
        }
    }
}

