using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchRoletteApi.Models
{
    public class SlackAttachment
    {
        public string text { get; set; }
    }
    public class SlackResult
    {
        public string response_type { get; set; }
        public string text { get; set; }
        public List<SlackAttachment> attachments { get; set; }
    }
}
