using System;
using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Persona
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        public string Email { get; set; }

        public string FechaFormato { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string NombreSimple { get; set; }

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

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}