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
    public partial class SYS_ParamInfo
    {
        public int ParamInfoID { get; set; }
        public string ParamName { get; set; }
        public string Title { get; set; }
        public string ParamKey { get; set; }
        public Nullable<int> ParamType { get; set; }
    
        public virtual SYS_ParamCurValue SYS_ParamCurValue { get; set; }
    }
    
}