using ManejoTareas.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManejoTareas {
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions options) : base(options) {
        }

        /* API FLuent */
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);

            /* modelBuilder.Entity<Tarea>().Property(t => t.Titulo)
                                        .HasMaxLength(250)
                                        .IsRequired(); */
        }

        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Paso> Pasos { get; set; }
        public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }
    }
}
