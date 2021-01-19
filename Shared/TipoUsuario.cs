using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class TipoUsuario
    {
        [Required(ErrorMessage = "Debe ingresar la descripción")]
        [MaxLength(ErrorMessage = "La longitud máxima es de 100")]
        public string Descripcion { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(ErrorMessage = "La longitud máxima es de 100")]
        public string Nombre { get; set; }
        public List<int> PaginasID { get; set; } = new List<int>();
    }
}