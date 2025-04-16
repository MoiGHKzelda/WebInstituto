using System.Linq;
using System.Collections.Generic;
using WebInstituto.Models;
using Microsoft.EntityFrameworkCore;

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
                IdProfesor = a.IdProfesor, 
                Profesor = a.Profesor ?? null
            })
            .ToList();
        }

        // Obtener una asignatura por Id
        public Asignatura GetById(int id)
        {
            return Db.Asignatura
                .Include(o => o.Horarios)
                .Include(p=>p.Profesor)
                .Include(a=>a.Alumnos)
                .ThenInclude(a=>a.Alumno)
                .FirstOrDefault(a => a.Id == id);
        }
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
        //Comprobar que no vuelva a repetir la asignatura por Nombre y Curso
        public bool ComprobarAsignaturaRepetida(Asignatura asignatura)
        {
            return Db.Asignatura.Any(a => a.Name == asignatura.Name && a.Course == asignatura.Course);
        }
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

