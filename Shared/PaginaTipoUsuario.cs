using System;
using System.Collections.Generic;
using System.Text;

namespace _01_MiPrimeraApp.Shared
{
    public class PaginaTipoUsuario
{
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreTipoUsuario { get; set; }

        public List<int> Buttons { get; set; } = new List<int>();
    }
}
