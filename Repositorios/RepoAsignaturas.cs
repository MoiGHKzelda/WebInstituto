using WebInstituto.Models;
using Microsoft.EntityFrameworkCore;

namespace WebInstituto.Repositorios
{
    public class RepoAsignaturas
    {
        // Contexto de base de datos.
        private readonly DBSqlite Db;

        // Constructor que inyecta la base de datos.
        public RepoAsignaturas(DBSqlite db)
        {
            this.Db = db;
        }

        // Obtiene todas las asignaturas de la base de datos.
        public IList<Asignatura> GetAll()
        {
            return Db.Asignatura
            .Select(a => new Asignatura
            {
                Id = a.Id,
                Name = a.Name,
                Course = a.Course,
                IdProfesor = a.IdProfesor,
                Profesor = a.Profesor ?? null
            })
            .ToList();
        }

        // Obtiene una asignatura por su Id incluyendo relaciones: horarios, profesor y alumnos.
        public Asignatura GetById(int id)
        {
            return Db.Asignatura
                .Include(o => o.Horarios)
                .Include(p => p.Profesor)
                .Include(a => a.Alumnos)
                .ThenInclude(a => a.Alumno)
                .FirstOrDefault(a => a.Id == id);
        }

        // Crea una nueva asignatura en la base de datos si no existe ya.
        public void CrearAsignatura(Asignatura nuevaAsignatura)
        {
            if (ComprobarAsignaturaRepetida(nuevaAsignatura))
            {
                Console.WriteLine("No se crea, asignatura ya existente");
            }
            else
            {
                Db.Asignatura.Add(nuevaAsignatura);
                Db.SaveChanges();
            }
        }

        // Comprueba si una asignatura con el mismo nombre y curso ya existe.
        public bool ComprobarAsignaturaRepetida(Asignatura asignatura)
        {
            return Db.Asignatura.Any(a => a.Name == asignatura.Name && a.Course == asignatura.Course);
        }

        // Actualiza una asignatura existente en la base de datos.
        public void ActualizarAsignatura(Asignatura asignatura)
        {
            var asignaturaExistente = Db.Asignatura.FirstOrDefault(a => a.Id == asignatura.Id);

            if (asignaturaExistente != null)
            {
                asignaturaExistente.Name = asignatura.Name;
                asignaturaExistente.Course = asignatura.Course;

                Db.SaveChanges();
            }
            else
            {
                throw new Exception("La asignatura no existe en la base de datos.");
            }
        }

        // Elimina una asignatura por su Id.
        public bool EliminarAsignatura(int id)
        {
            var asignatura = Db.Asignatura.Find(id);
            if (asignatura != null)
            {
                Db.Asignatura.Remove(asignatura);
                Db.SaveChanges();
                return true;  // Éxito
            }
            return false;  // No encontrada
        }
    }
}
