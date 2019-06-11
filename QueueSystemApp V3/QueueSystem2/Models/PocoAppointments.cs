using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoAppointments
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int user_id { get; set; }
        public int branch_id { get; set; }
        public string branch_location { get; set; }
        public string res_name { get; set; }
        public int days { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int seconds { get; set; }

    }
}