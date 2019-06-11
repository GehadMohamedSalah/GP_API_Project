using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoOrganizations
    {
        public int id { get; set; }
        public string name { get; set; }
        public int manager_id { get; set; }
        public string manager_name { get; set; }
    }
}