using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Autor
    {
        [Required(ErrorMessage = "Debe ingresar la descripción del autor")]
        [MaxLength(500, ErrorMessage = "La longitud máxima debe ser 500")]
        [MinLength(10, ErrorMessage = "La longitud mínima debe ser 10")]
        public string Descripcion { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país")]
        public string IdPais { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un sexo")]
        public string IdSexo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del autor")]
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string Nombre { get; set; }

        public string Pais { get; set; }

        [Required(ErrorMessage = "Debe ingresar el primer apellido del autor")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(3, ErrorMessage = "La longitud mínima debe ser 2")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar el primer apellido del autor")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(3, ErrorMessage = "La longitud mínima debe ser 2")]
        public string SegundoApellido { get; set; }

        public string Sexo { get; set; }
    }
}