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
    public partial class SYS_ParamCurValue
    {
        public int ParamInfoID { get; set; }
        public Nullable<double> DoubleValue { get; set; }
        public Nullable<bool> State { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    
        public virtual SYS_ParamInfo SYS_ParamInfo { get; set; }
    }
    
}