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
            Console.WriteLine("Hola");
            // Marca la asignatura como modificada
            Db.Entry(asignatura).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            Db.SaveChanges();
        }

    }
}

