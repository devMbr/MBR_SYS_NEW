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
    public class ANInfoModel
    {
        /// <summary>
        /// 透水率高点曲线
        /// </summary>
        public  AN_HighCurve HighCurve { get; set; }

        /// <summary>
        /// 透水率低点曲线
        /// </summary>
        public AN_LowCurve LowCurve { get; set; }

        /// <summary>
        /// 累积氯曲线
        /// </summary>
        public AN_CLCurve CLCurve { get; set; }
    }
    
}
