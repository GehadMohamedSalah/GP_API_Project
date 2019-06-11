using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoReqDocs
    {
        public int id { get; set; }
        public int doc_id { get; set; }
        public string doc_name { get; set; }
        public int service_id { get; set; }
        public string service_name { get; set; }
        public string notes { get; set; }
    }
}