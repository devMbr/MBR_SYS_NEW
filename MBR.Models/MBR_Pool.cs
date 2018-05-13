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
    public partial class MBR_Pool
    {
        public MBR_Pool()
        {
            this.MBR_Info = new HashSet<MBR_Info>();
            this.SYS_PoisonPump = new HashSet<SYS_PoisonPump>();
            this.SYS_RuningParam = new HashSet<SYS_RuningParam>();
            this.WN_WarningLog = new HashSet<WN_WarningLog>();
            this.WN_WarningSett = new HashSet<WN_WarningSett>();
            this.MBR_RunningInfo = new HashSet<MBR_RunningInfo>();
        }
    
        public int PoolID { get; set; }
        public Nullable<double> Area { get; set; }
        public string PoolName { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> Order { get; set; }
    
        public virtual AN_NetInfo AN_NetInfo { get; set; }
        public virtual ICollection<MBR_Info> MBR_Info { get; set; }
        public virtual SYS_AirValve SYS_AirValve { get; set; }
        public virtual ICollection<SYS_PoisonPump> SYS_PoisonPump { get; set; }
        public virtual ICollection<SYS_RuningParam> SYS_RuningParam { get; set; }
        public virtual WN_PoolStatus WN_PoolStatus { get; set; }
        public virtual ICollection<WN_WarningLog> WN_WarningLog { get; set; }
        public virtual ICollection<WN_WarningSett> WN_WarningSett { get; set; }
        public virtual ICollection<MBR_RunningInfo> MBR_RunningInfo { get; set; }
    }
    
}
