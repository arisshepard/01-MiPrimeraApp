using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class TipoLibro
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre del tipo de libro")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar la descripción del tipo de libro")]
        public string Descripcion { get; set; }

    }
}