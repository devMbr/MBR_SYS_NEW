using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using MBR.Models;

namespace MBR.Web.Models
{
    public class WarningSettModel
    {
        /// <summary>
        /// 预警设置
        /// </summary>
        public Settings Settings { get; set; }

        /// <summary>
        /// 预警规则
        /// </summary>
        public List<WN_WarningSett> WarningSetts { get; set; }
    }
    
}
