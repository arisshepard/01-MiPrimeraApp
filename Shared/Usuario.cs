using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Usuario
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Debe introducir el usuario")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe introducir la contraseña")]
        public string Password { get; set; }
    }
}