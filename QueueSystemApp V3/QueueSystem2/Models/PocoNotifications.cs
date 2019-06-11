using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoNotifications
    {
        public int id { get; set; }
        public string msg { get; set; }
        public bool seen { get; set; }
        public DateTime dateTime { get; set; }
        public string type_noti { get; set; }
        public int type_noti_id { get; set; }
        public int client_id { get; set; }
        public int emp_id { get; set; }
    }

}