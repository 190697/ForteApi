using Microsoft.EntityFrameworkCore;
using TareasApi.Entities;

namespace TareasApi.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Estatus> Estatus { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración explícita de la relación (opcional, EF Core lo infiere automáticamente)
            modelBuilder.Entity<Tarea>()
                .HasOne(c => c.Estatus)
                .WithMany(p => p.Tareas)
                .HasForeignKey(c => c.Id_Estatus);
        }
    }
}
