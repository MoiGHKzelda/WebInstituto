using WebInstituto.Models;

namespace WebInstituto.Repositorios
{
    public class RepoHorario
    {
        private readonly DBSqlite Db;
        public RepoHorario(DBSqlite db)
        {
            this.Db = db;
        }
        public IList<Horario> GetAll()
        {
            return Db.Horario.ToList();
        }

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
        public bool ComprobarFechaValida(Horario horario)
        {
            
            DateTime comienzo = DateTime.Parse(horario.Start);
            DateTime fin = DateTime.Parse(horario.End);
            // Horas no válidas
            if (comienzo >= fin)
            {
                return false;
            }

            IList<Horario> horariosMismoId = Db.Horario.Where(h => h.AsignaturaId == horario.AsignaturaId).ToList();
            foreach(Horario h in horariosMismoId)
            {
                DateTime hComienzo = DateTime.Parse(h.Start);
                DateTime hFin = DateTime.Parse(h.End);
                // Horarios solapados
                if (comienzo < hFin && fin > hComienzo && horario.Day==h.Day)
                {
                    return false;  
                }
            }

            return true;
        }

        //Actualizar horario
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

        //Eliminar Horario
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

        //Buscar horario por id
        public Horario GetById(int id)
        {
            return Db.Horario.FirstOrDefault(h => h.Id == id);
        }

    }
}
