using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace WebInstituto.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Persona
    {
        /// <summary>
        /// Atributos
        /// </summary>
        /// 
        [Key]
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public int? Teacher { get; set; }

        [Required]
        public string? EmergencyPhone { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string ContrasenyaHash { get; set; }

        // Clave foránea para Asignatura
        public int? AsignaturaId { get; set; }
        [ForeignKey("AsignaturaId")]
        public Asignatura Asignatura { get; set; }
        public IList<AsignaturaPersona> AsignaturasAlumno { get; set; }
        public IList<Asignatura> AsignaturasImpartidas { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Persona()
        {
            AsignaturasAlumno = new List<AsignaturaPersona>();
            AsignaturasImpartidas = new List<Asignatura>();
        }
        public Persona(string name, string lastName, int teacher, string telf, string mail, string passwd) 
        {
            this.Name= name;
            this.LastName= lastName;
            this.Teacher = teacher;
            this.EmergencyPhone = telf;
            this.Mail = mail;
            this.ContrasenyaHash = passwd;
        }
    }
}


