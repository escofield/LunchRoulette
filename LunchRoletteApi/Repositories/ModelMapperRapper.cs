using NHibernate.Mapping.ByCode;
using NHibernate.Cfg.MappingSchema;
using LunchRoletteApi.Repositories.Mapping;

namespace LunchRoletteApi.Repositories
{
    public static class ModelMapperRapper 
    {
       public static HbmMapping CompileMapping()
        {
            var mm = new ModelMapper();
            mm.AddMapping<LocationMapping>();
            mm.AddMapping<TodaysLunchMapping>();
            return mm.CompileMappingForAllExplicitlyAddedEntities();
        }  
    }
}
