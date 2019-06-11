using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoServices
    {
        public int id { get; set; }
        public string name { get; set; }
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public TimeSpan period { get; set; }
        public TimeSpan break_start { get; set; }
        public TimeSpan break_period { get; set; }
        public int org_id { get; set; }
    }
}