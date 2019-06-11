using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoRequest
    {
        public int id { get; set; }
        public DateTime dateTime { get; set; }
        public int client_id { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; }
        public int service_id { get; set; }
        public string service_name { get; set; }
        public string state { get; set; }
        public int branch_id { get; set; }
        public string branch_location { get; set; }
        public List<string> reqatt { get; set; }
        public string req_name { get; set; }
    }
}