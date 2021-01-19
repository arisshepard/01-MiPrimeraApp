using _01_MiPrimeraApp.Server.Mappers;
using System;
using System.Collections.Generic;

namespace _01_MiPrimeraApp.Server.Services
{
    public class TipoUsuarioService
    {
    }

    public interface ITipoUsuarioMappingService : IMapper<Models.TipoUsuario, Shared.TipoUsuario>, IMapper<IEnumerable<Models.TipoUsuario>, IEnumerable<Shared.TipoUsuario>>
    {
    }

    public class TipoUsuarioMappingService : ITipoUsuarioMappingService
    {
        private readonly IMapper<Models.TipoUsuario, Shared.TipoUsuario> _entidadDbTOentidadIMapper;
        private readonly IMapper<IEnumerable<Models.TipoUsuario>, IEnumerable<Shared.TipoUsuario>> _entidadDbEnumerableTOentidadEnumerableIMapper;

        public TipoUsuarioMappingService(IMapper<Models.TipoUsuario, Shared.TipoUsuario> entidadDbTOentidadIMapper, 
            IMapper<IEnumerable<Models.TipoUsuario>, IEnumerable<Shared.TipoUsuario>> entidadDbEnumerableTOentidadEnumerableIMapper)
        {
            _entidadDbEnumerableTOentidadEnumerableIMapper = entidadDbEnumerableTOentidadEnumerableIMapper;
            _entidadDbTOentidadIMapper = entidadDbTOentidadIMapper;
        }

        public Shared.TipoUsuario Map(Models.TipoUsuario entity)
        {
            return _entidadDbTOentidadIMapper.Map(entity);
        }

        public IEnumerable<Shared.TipoUsuario> Map(IEnumerable<Models.TipoUsuario> entities)
        {
            return _entidadDbEnumerableTOentidadEnumerableIMapper.Map(entities);
        }
    }
}