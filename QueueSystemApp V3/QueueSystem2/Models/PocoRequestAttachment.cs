using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoRequestAttachment
    {
        public int id { get; set; }
        public string path { get; set; }
        public int req_id { get; set; }
    }
}