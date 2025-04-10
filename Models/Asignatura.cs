using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebInstituto.Models;

namespace WebInstituto.Models
{
    public class Asignatura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Course { get; set; }

        // Relación con Persona (Alumnos)
        public virtual IList<AsignaturaPersona> Alumnos { get; set; }

        // Relación con Profesor
        [ForeignKey("IdProfesor")]
        public Persona? Profesor { get; set; }

        public int? IdProfesor { get; set; }

        // Relación con horarios
        public virtual IList<Horario> Horarios { get; set; }

        public Asignatura()
        {
            Alumnos = new List<AsignaturaPersona>();
            Horarios = new List<Horario>();
        }

        public Asignatura(string name, int course)
        {
            Name = name;
            Course = course;
        }
    }
}

