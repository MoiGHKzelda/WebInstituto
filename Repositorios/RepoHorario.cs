using WebInstituto.Models;

namespace WebInstituto.Repositorios
{
    public class RepoHorario
    {
        // Contexto de base de datos.
        private readonly DBSqlite Db;

        // Constructor que inyecta el contexto.
        public RepoHorario(DBSqlite db)
        {
            this.Db = db;
        }

        // Obtiene todos los horarios almacenados.
        public IList<Horario> GetAll()
        {
            return Db.Horario.ToList();
        }

        // Crea un nuevo horario si la validación es correcta.
        public Horario CrearHorario(Horario horario)
        {
            if (!ComprobarFechaValida(horario))
            {
                Console.WriteLine("No se crea, está repetido o no son válidos los datos");
                return null;
            }
            Db.Horario.Add(horario);
            Db.SaveChanges();
            return horario;
        }

        // Comprueba si el horario es válido (sin solaparse y con horas coherentes).
        public bool ComprobarFechaValida(Horario horario)
        {
            DateTime comienzo = DateTime.Parse(horario.Start);
            DateTime fin = DateTime.Parse(horario.End);

            // Comprobación de horas: la hora de inicio debe ser menor a la de fin.
            if (comienzo >= fin)
            {
                return false;
            }

            // Verificar solapamientos con horarios ya existentes en el mismo día y asignatura.
            IList<Horario> horariosMismoId = Db.Horario.Where(h => h.AsignaturaId == horario.AsignaturaId).ToList();
            foreach (Horario h in horariosMismoId)
            {
                DateTime hComienzo = DateTime.Parse(h.Start);
                DateTime hFin = DateTime.Parse(h.End);

                if (comienzo < hFin && fin > hComienzo && horario.Day == h.Day)
                {
                    return false;
                }
            }

            return true;
        }

        // Actualiza los datos de un horario existente.
        public void ActualizarHorario(Horario horario)
        {
            var horarioExistente = Db.Horario.FirstOrDefault(h => h.Id == horario.Id);
            if (horarioExistente != null)
            {
                horarioExistente.Start = horario.Start;
                horarioExistente.End = horario.End;
                horarioExistente.Day = horario.Day;
                Db.SaveChanges();
            }
        }

        // Elimina un horario por su Id.
        public bool EliminarHorario(int id)
        {
            var horario = Db.Horario.Find(id);
            if (horario != null)
            {
                Db.Horario.Remove(horario);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        // Obtiene un horario específico por su Id.
        public Horario GetById(int id)
        {
            return Db.Horario.FirstOrDefault(h => h.Id == id);
        }
    }
}
