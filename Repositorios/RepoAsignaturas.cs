using System.Linq;
using System.Collections.Generic;
using WebInstituto.Models;

namespace WebInstituto.Repositorios
{
    public class RepoAsignaturas
    {
        private readonly DBSqlite Db;

        public RepoAsignaturas(DBSqlite db)
        {
            this.Db = db;
        }

        public IList<Asignatura> GetAll()
        {
            return Db.Asignatura
            .Select(a => new Asignatura
            {
                Id = a.Id,
                Name = a.Name,
                Course = a.Course,
                IdProfesor = a.IdProfesor, // Puede ser null
                Profesor = a.Profesor ?? null // Esto asegura que no haya excepciones
            })
            .ToList();
        }

        // Obtener una asignatura por Id
        public Asignatura GetById(int id)
        {
            return Db.Asignatura.FirstOrDefault(a => a.Id == id);
        }
        public void CrearAsignatura(Asignatura nuevaAsignatura)
        {
            Db.Asignatura.Add(nuevaAsignatura);
            Db.SaveChanges();
        }
    }
}
