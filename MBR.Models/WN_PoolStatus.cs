//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MBR.Models
{
    public partial class WN_PoolStatus
    {
        public int PoolID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    
        public virtual MBR_Pool MBR_Pool { get; set; }
    }
    
}