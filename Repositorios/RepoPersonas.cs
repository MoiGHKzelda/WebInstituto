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
            persona.ContrasenyaHash=SeguridadService.HashPassword(persona.ContrasenyaHash);
            Persona personaNew = Db.Persona.Add(persona).Entity;

            Db.SaveChanges();

            return persona;
        }
        public Persona GetByEmail(string mail )
        {
            var hola = Db.Persona;
            
            IList<Persona> lista = this.Db.Persona.ToList();
            Persona personaMail = lista.Where(p => p.Mail == mail).FirstOrDefault();
            return personaMail;
        }
    }
}
