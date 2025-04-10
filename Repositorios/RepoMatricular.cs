using WebInstituto.Models;
using Microsoft.EntityFrameworkCore;

namespace WebInstituto.Repositorios
{
    public class RepoMatricular
    {
        private readonly DBSqlite Db;

        public RepoMatricular(DBSqlite db)
        {
            this.Db = db;
        }

        // Método para crear una matrícula
        public AsignaturaPersona crearMatriculacion(AsignaturaPersona personaMatricular)
        {
            // Agregamos la nueva matrícula a la base de datos
            Db.AsignaturaPersona.Add(personaMatricular);
            Db.SaveChanges();
            return personaMatricular;
        }

        // Método para eliminar una matrícula
        public void eliminarMatriculacion(AsignaturaPersona personaMatricular)
        {
            var matricula = Db.AsignaturaPersona
                .FirstOrDefault(m => m.IdAsignatura == personaMatricular.IdAsignatura && m.IdAlumno == personaMatricular.IdAlumno);

            if (matricula != null)
            {
                Db.AsignaturaPersona.Remove(matricula);
                Db.SaveChanges();
            }
        }

        // Método para verificar si el estudiante ya está matriculado en la asignatura
        public bool ExisteMatriculacion(AsignaturaPersona personaMatricular)
        {
            return Db.AsignaturaPersona
                .Any(m => m.IdAsignatura == personaMatricular.IdAsignatura && m.IdAlumno == personaMatricular.IdAlumno);
        }
    }
}
