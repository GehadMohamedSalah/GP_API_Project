using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueueSystem2.Models
{
    public class PocoClient
    {
        public int id { get; set; }
        public bool block { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string ssn { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public int person_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}