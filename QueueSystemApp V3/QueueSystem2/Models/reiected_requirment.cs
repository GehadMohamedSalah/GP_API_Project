//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QueueSystem2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class reiected_requirment
    {
        public int id { get; set; }
        public string notes { get; set; }
        public Nullable<int> request_id { get; set; }
    
        public virtual Request Request { get; set; }
    }
}
