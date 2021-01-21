using _01_MiPrimeraApp.Server.Mappers;
using System.Collections.Generic;

namespace _01_MiPrimeraApp.Server.Services
{
    public interface IEntityMappingService<TEntityDB, TEntity> : IMapper<TEntityDB, TEntity>, IMapper<IEnumerable<TEntityDB>, IEnumerable<TEntity>>
    {
    }

    public class EntityMappingService<TEntityDB, TEntity> : IEntityMappingService<TEntityDB, TEntity>
    {
        private readonly IMapper<TEntityDB, TEntity> _entidadDBTOentidad;
        private readonly IMapper<IEnumerable<TEntityDB>, IEnumerable<TEntity>> _entidadEnumDBTOEntidadEnum;

        public EntityMappingService(IMapper<TEntityDB, TEntity> entidadDBTOentidad,
            IMapper<IEnumerable<TEntityDB>, IEnumerable<TEntity>> entidadEnumDBTOEntidadEnum)
        {
            _entidadDBTOentidad = entidadDBTOentidad;
            _entidadEnumDBTOEntidadEnum = entidadEnumDBTOEntidadEnum;
        }

        public TEntity Map(TEntityDB entity)
        {
            return _entidadDBTOentidad.Map(entity);
        }

        public IEnumerable<TEntity> Map(IEnumerable<TEntityDB> entities)
        {
            return _entidadEnumDBTOEntidadEnum.Map(entities);
        }
    }

    public class GenericService
    {
    }
}