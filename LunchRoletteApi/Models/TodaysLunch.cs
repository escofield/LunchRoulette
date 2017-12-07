using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchRoletteApi.Models
{
    public class TodaysLunch
    {
        public int TodaysLunchId { get; set; }
        public DateTime LunchDate { get; set; }
        public string DisplayName { get; set; }
        public int UpCount { get; set; }
        public int DownCount { get; set; }

        public TodaysLunch()
        {
            LunchDate = DateTime.Today;
        }
    }
}
