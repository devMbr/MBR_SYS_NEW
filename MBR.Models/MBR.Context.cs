﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MBR.Models
{
    public partial class MBREntities : DbContext
    {
        public MBREntities()
            : base("name=MBREntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AirValve> AirValve { get; set; }
        public DbSet<AN_CLCurve> AN_CLCurve { get; set; }
        public DbSet<AN_HighCurve> AN_HighCurve { get; set; }
        public DbSet<AN_LowCurve> AN_LowCurve { get; set; }
        public DbSet<AN_NetInfo> AN_NetInfo { get; set; }
        public DbSet<CIPPump> CIPPump { get; set; }
        public DbSet<Knowlege> Knowlege { get; set; }
        public DbSet<MBR_Info> MBR_Info { get; set; }
        public DbSet<MBR_Pool> MBR_Pool { get; set; }
        public DbSet<MBR_RunningInfo> MBR_RunningInfo { get; set; }
        public DbSet<MBR_WashingInfo> MBR_WashingInfo { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<ModulePermission> ModulePermission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SYS_CIPLog> SYS_CIPLog { get; set; }
        public DbSet<SYS_ParamCurValue> SYS_ParamCurValue { get; set; }
        public DbSet<SYS_ParamInfo> SYS_ParamInfo { get; set; }
        public DbSet<SYS_PoisonLog> SYS_PoisonLog { get; set; }
        public DbSet<SysException> SysException { get; set; }
        public DbSet<SysLog> SysLog { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<WarningStat> WarningStat { get; set; }
        public DbSet<WN_ParamSett> WN_ParamSett { get; set; }
        public DbSet<WN_WarningCond> WN_WarningCond { get; set; }
        public DbSet<WN_WarningLog> WN_WarningLog { get; set; }
        public DbSet<WN_WarningSett> WN_WarningSett { get; set; }
    }
}
