using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInstituto.Models
{
    public class AsignaturaPersona
    {
        [Key]
        public int Id { get; set; }

        public int IdAsignatura { get; set; }
        [ForeignKey("IdAsignatura")]
        public Asignatura Asignatura { get; set; }

        public int IdAlumno { get; set; }
        [ForeignKey("IdAlumno")]
        public Persona Alumno { get; set; }
        
    }

}
