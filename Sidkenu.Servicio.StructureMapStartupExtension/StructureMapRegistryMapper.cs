using AutoMapper;
using StructureMap;

namespace Sidkenu.Servicio.StructureMapStartupExtension
{
    public class StructureMapRegistryMapper : Registry
    {
        public StructureMapRegistryMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Mapper.SeguridadAutoMapperProfile>();
                cfg.AddProfile<Mapper.CoreAutoMapperProfile>();
            });

            For<IMapper>().Use(mapperConfiguration.CreateMapper());
        }
    }
}
