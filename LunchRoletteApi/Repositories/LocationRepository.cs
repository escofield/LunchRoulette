using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using IO.Swagger.Models;

namespace LunchRoletteApi.Repositories
{
    public class LocationRepository
    {
        private ISession _session;

        public LocationRepository()
        {                                            
            _session = SessionFactoryProvider.GetCurrentSession();
        }

        public IList<Location> GetLocations()
        {
            return _session.QueryOver<Location>().List();
        }

        public Location GetLunch()
        {
            var locations = GetLocations();
            var random = new Random().Next(0, locations.Count - 1);
            return locations[random];
        }

       
    }
}
