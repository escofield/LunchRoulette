using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using LunchRoletteApi.Models;

namespace LunchRoletteApi.Repositories.Mapping
{
    public class TodaysLunchMapping : ClassMapping<TodaysLunch>
    {
        public TodaysLunchMapping()
        {
            Table("TodaysLunch");
            Id(t => t.TodaysLunchId, map => map.Column("TODAYSLUNCHID"));
            Property(t => t.DisplayName, map => map.Column("DISPLAYNAME"));
            Property(t => t.LunchDate, map => map.Column("LUNCHDATE"));
            Property(t => t.DownCount, map => map.Column("DOWNCOUNT"));
            Property(t => t.UpCount, map => map.Column("UPCOUNT"));
            Lazy(false);
        }

    }
}