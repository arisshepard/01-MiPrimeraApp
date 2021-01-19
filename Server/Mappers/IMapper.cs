namespace _01_MiPrimeraApp.Server.Mappers
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource entity);
    }
}