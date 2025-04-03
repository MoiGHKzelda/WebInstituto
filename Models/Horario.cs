using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInstituto.Models
{
        public class Horario
        {[Key]
        public int Id { get; set; }

        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

        // La propiedad que representa la asignatura, que se llama 'Subject'
        public int AsignaturaId { get; set; }

        // Relación con la asignatura
        [ForeignKey("AsignaturaId")]
        public Asignatura Asignatura { get; set; }  // Relacionamos con 'Asignatura'

        public Horario() { }
    }
}
