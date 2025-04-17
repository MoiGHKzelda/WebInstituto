using WebInstituto.Models;

namespace WebInstituto.Repositorios
{
    public class RepoMatricular
    {
        // Contexto de base de datos.
        private readonly DBSqlite Db;

        // Constructor que recibe el contexto.
        public RepoMatricular(DBSqlite db)
        {
            this.Db = db;
        }

        // Crea una nueva matrícula de alumno en una asignatura.
        public AsignaturaPersona CrearMatriculacion(AsignaturaPersona personaMatricular)
        {
            Db.AsignaturaPersona.Add(personaMatricular);
            Db.SaveChanges();
            return personaMatricular;
        }

        // Elimina una matrícula existente.
        public void EliminarMatriculacion(AsignaturaPersona personaMatricular)
        {
            var matricula = Db.AsignaturaPersona
                .FirstOrDefault(m => m.IdAsignatura == personaMatricular.IdAsignatura && m.IdAlumno == personaMatricular.IdAlumno);

            if (matricula != null)
            {
                Db.AsignaturaPersona.Remove(matricula);
                Db.SaveChanges();
            }
        }

        // Verifica si un alumno ya está matriculado en una asignatura.
        public bool ExisteMatriculacion(AsignaturaPersona personaMatricular)
        {
            return Db.AsignaturaPersona
                .Any(m => m.IdAsignatura == personaMatricular.IdAsignatura && m.IdAlumno == personaMatricular.IdAlumno);
        }
    }
}
