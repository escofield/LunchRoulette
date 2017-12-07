using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchRoletteApi.Models
{
    public static class ParameterExtension
    {
        public static Parameter FindParameter(this List<Parameter> o, string name)
        {
            return o.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }
    }
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public static List<Parameter> ParseParameters(string list)
        {
            list = list ?? "";
            return list.Split('-').Where(r => r.Length > 0).Select(r =>
            {
                
                var firstSpace = r.IndexOf(' ');
                if (firstSpace <= 0) return new Parameter() { Name = r };
                return new Parameter()
                {
                    Name = r.Substring(0, firstSpace ).Trim(),
                    Value = r.Substring(firstSpace, r.Length - firstSpace).Trim()
                };
            }).ToList();
        }
    }
}
