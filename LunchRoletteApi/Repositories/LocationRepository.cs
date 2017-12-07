
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using IO.Swagger.Models;
using LunchRoletteApi.Models;

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

        public string GetLunch()
        {
            var todaysLunch = _session.QueryOver<TodaysLunch>().List().FirstOrDefault(x => x.LunchDate.ToShortDateString() == DateTime.Today.ToShortDateString());
            if (todaysLunch == null)
            {
                var locations = GetLocations();
                var random = new Random().Next(0, locations.Count - 1);
                var lunchLocation = locations[random];
                var lunches = _session.QueryOver<TodaysLunch>().List().Select(x => x.TodaysLunchId);
                var newlunchId = lunches.Any() ? lunches.Max() + 1 : 1;
                todaysLunch = new TodaysLunch()
                {
                    TodaysLunchId = newlunchId,
                    DisplayName = lunchLocation.DisplayName,
                    DownCount = 0,
                    UpCount = 0
                };
                _session.SaveOrUpdate(todaysLunch);
                _session.BeginTransaction().Commit();
            }
            return todaysLunch.DisplayName;
        }
       
        public void AddLunch(string name, string description)
        {
            var newLocation = new Location
            {
                LocationId = GetLocations().Select(l => l.LocationId).Max() + 1,
                DisplayName = name,
                Description = description
            };

            _session.SaveOrUpdate(newLocation);
            _session.BeginTransaction().Commit();

           
            
        }
       
    }
}
