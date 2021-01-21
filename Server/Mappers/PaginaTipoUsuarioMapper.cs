using _01_MiPrimeraApp.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace _01_MiPrimeraApp.Server.Mappers
{
    public class PaginaTipoUsuarioMapper : IMapper<PaginaTipoUsuario, Shared.PaginaTipoUsuario>
    {
        public Shared.PaginaTipoUsuario Map(PaginaTipoUsuario entity)
        {
            Shared.PaginaTipoUsuario paginaTipoUsuario = new Shared.PaginaTipoUsuario()
            {
                Buttons = entity.PaginaTipoUsuButton == null ? new List<int>() : entity.PaginaTipoUsuButton.Select(button => (int)button.Iidbutton).ToList(),
                ID = entity.Iidpaginatipousuario,
                Nombre = entity.IidpaginaNavigation.Mensaje,
                NombreTipoUsuario = entity.IidtipousuarioNavigation.Nombre
            };

            return paginaTipoUsuario;
        }
    }
}