using System.Data.SQLite;
using Proyecto.Services;
using WebInstituto.Models;


namespace WebInstituto.Repositorios
{
    public class RepoPersonas
    {
        private readonly DBSqlite Db;
        public RepoPersonas(DBSqlite db)
        {
            this.Db = db;
        }
        public IList<Persona> GetAll()
        {
            return Db.Persona.ToList();
        }
        public Persona CrearPersona(Persona persona)
        {
            if (!Db.Persona.Any(a => a.Mail == persona.Mail))
            {
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
        public Persona GetByEmail(string mail )
        {        
            IList<Persona> lista = this.Db.Persona.ToList();
            Persona personaMail = lista.Where(p => p.Mail == mail).FirstOrDefault();
            return personaMail;
        }

    }
}
