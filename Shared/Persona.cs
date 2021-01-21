using System;
using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Persona
    {
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        public string Email { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string FechaFormato { get; set; }
        public int ID { get; set; }
        [Required(ErrorMessage = "Debe ingresar el tipo de usuario")]
        public string IDTipoUsuario { get; set; }

        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string NombreSimple { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del usuario")]
        public string NombreUsuario { get; set; }
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
        public string PasswordUsuario { get; set; } = "12345678";

        [Required(ErrorMessage = "Debe ingresar el primer apellido")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar el segundo apellido")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string SegundoApellido { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima debe ser 50")]
        public string Telefono { get; set; }
    }
}