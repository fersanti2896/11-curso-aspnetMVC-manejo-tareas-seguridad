using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ManejoTareas.Entities {
    public class Tarea {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public string UsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Paso> Pasos { get; set; }
        public List<ArchivoAdjunto> ArchivoAdjuntos { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
