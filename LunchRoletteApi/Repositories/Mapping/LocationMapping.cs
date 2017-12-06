using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using IO.Swagger.Models;

namespace LunchRoletteApi.Repositories.Mapping
{
    public class LocationMapping  : ClassMapping<Location>
    {
         public LocationMapping()
        {
            Table("Location");
            Id(t => t.LocationId, map => map.Column("LOCATIONID"));
            Property(t => t.DisplayName, map => map.Column("DISPLAYNAME"));
            Property(t => t.Description, map => map.Column("DESCRIPTION"));
            Lazy(false);
        }                                                      
    }
}
