using Microsoft.EntityFrameworkCore;
using Proyecto.Services;
using WebInstituto.Models;

namespace WebInstituto
{
    public class DBSqlite : DbContext
    {
        private readonly string _pathDB;

        public DBSqlite()
        {
            _pathDB = GetPath.GetDatabasePath();
        }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Asignatura> Asignatura { get; set; }
        public DbSet<Horario> Horario { get; set; }
        public DbSet<AsignaturaPersona> AsignaturaPersona { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite($"Data Source={_pathDB}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Asignatura - Horario N:1
            modelBuilder.Entity<Asignatura>().HasMany(a => a.Horarios).WithOne(h => h.Asignatura).HasForeignKey(h=>h.AsignaturaId);

            // Asignatura - Persona (Profesor) :N
            modelBuilder.Entity<Persona>().HasMany(a => a.AsignaturasImpartidas).WithOne(h => h.Profesor).HasForeignKey(h => h.IdProfesor);

            modelBuilder.Entity<AsignaturaPersona>().HasOne(p => p.Alumno).WithMany(a => a.AsignaturasAlumno).HasForeignKey(p=>p.IdAlumno);
            modelBuilder.Entity<AsignaturaPersona>().HasOne(a => a.Asignatura).WithMany(p => p.Alumnos).HasForeignKey(p => p.IdAsignatura);
        }


    }

}
