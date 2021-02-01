using System;
using System.ComponentModel.DataAnnotations;

namespace _01_MiPrimeraApp.Shared
{
    public class Reserva
    {
        [Required(ErrorMessage = "Debe ingresar la cantidad")]
        public int? Cantidad { get; set; }

        public DateTime FechaFin { get; set; } = DateTime.Now;
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Debe ingresar el libro")]
        public int? IDLibro { get; set; }

        [Required(ErrorMessage = "Debe ingresar el usuario")]
        public string IDUsuario { get; set; }

        public string NombreLibro { get; set; }
        public string NombreUsuario { get; set; }

        public string FechaInicioCadena { get; set; }
        public string FechaFinCadena { get; set; }

        public int? ID { get; set; }
    }
}