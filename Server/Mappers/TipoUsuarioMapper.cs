using _01_MiPrimeraApp.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace _01_MiPrimeraApp.Server.Mappers
{
    public class TipoUsuarioMapper : IMapper<TipoUsuario, Shared.TipoUsuario>,
        IMapper<Shared.TipoUsuario, TipoUsuario>
    {
        public Shared.TipoUsuario Map(TipoUsuario entity)
        {
            Shared.TipoUsuario tipoUsuario = new Shared.TipoUsuario()
            {
                Descripcion = entity.Descripcion,
                Nombre = entity.Nombre,
                ID = entity.Iidtipousuario,
                PaginasID = entity.PaginaTipoUsuario == null ? new List<int>() : entity.PaginaTipoUsuario.Select(pagina => (int)pagina.Iidpagina).ToList()
            };

            return tipoUsuario;
        }

        public TipoUsuario Map(Shared.TipoUsuario entity)
        {
            TipoUsuario tipoUsuario = new TipoUsuario()
            {
                Descripcion = entity.Descripcion,
                Iidtipousuario = entity.ID,
                Nombre = entity.Nombre
            };

            return tipoUsuario;
        }
    }
}