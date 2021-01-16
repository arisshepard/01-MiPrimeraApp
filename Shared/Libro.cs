using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Libro
    {
        public string Fotocaratula { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessage = "Ingrese Autor")]
        public string IDAutor { get; set; }
        public string Autor { get; set; }
        public string Libropdf { get; set; }

        [Required(ErrorMessage = "Ingrese número de páginas")]
        public int Numpaginas { get; set; }

        [Required(ErrorMessage = "Ingrese Resumen")]
        public string Resumen { get; set; }

        [Required(ErrorMessage = "Ingrese stock")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Ingrese Título")]
        public string Titulo { get; set; }
    }
}