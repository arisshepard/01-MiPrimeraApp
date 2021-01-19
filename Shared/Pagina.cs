using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Pagina
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de la acción que dirá")]
        [MaxLength(ErrorMessage = "La longitud máxima debe ser 100")]
        public string Accion { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "Debe ingresar el mensaje que dirá")]
        [MaxLength(ErrorMessage = "La longitud máxima debe ser 100")]
        public string Mensaje { get; set; }
        public string NombreVisible { get; set; }
        public bool Visible { get; set; }
    }
}