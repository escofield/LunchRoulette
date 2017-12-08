
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

        private TodaysLunch TodaysLunch()
        {
            return _session.QueryOver<TodaysLunch>().List().FirstOrDefault(x => x.LunchDate.ToShortDateString() == DateTime.Today.ToShortDateString());
        }
        public (TodaysLunch, Location) GetLunch()
        {
            var todaysLunch = TodaysLunch();
            Location lunchLocation;
            if (todaysLunch == null)
            {
                var locations = GetLocations();
                var random = new Random().Next(0, locations.Count - 1);
                lunchLocation = locations[random];
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
            else
            {
                var locations = GetLocations();
                lunchLocation = _session.QueryOver<Location>().List().FirstOrDefault(x => x.DisplayName == todaysLunch.DisplayName);
            }
            return (todaysLunch,lunchLocation) ;
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

        public bool VoteUp()
        {
            var todaysLunch = TodaysLunch();
            if (todaysLunch != null)
            {
                todaysLunch.UpCount++;
                _session.SaveOrUpdate(todaysLunch);
                _session.BeginTransaction().Commit();
                return true;
            }
            return false;
        }

        public bool VoteDown()
        {
            var todaysLunch = TodaysLunch();
            if (todaysLunch != null)
            {
                todaysLunch.DownCount++;
                _session.SaveOrUpdate(todaysLunch);
                _session.BeginTransaction().Commit();
                return true;
            }
            return false;
        }
       
    }
}
