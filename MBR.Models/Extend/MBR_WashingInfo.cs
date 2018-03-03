using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(MBR_WashingInfoMetadata))]
    public partial class MBR_WashingInfo
    {

    }

    public class MBR_WashingInfoMetadata
    {

        [Required]
        [Display(Name = "开时时间")]
        public Nullable<System.DateTime> BeginTime { get; set; }

        [Required]
        [Display(Name = "结束时间")]
        public Nullable<System.DateTime> EndTime { get; set; }

        [Required]
        [Display(Name = "清洗类型")]
        public Nullable<int> WashType { get; set; }

        [Display(Name = "药剂类型")]
        public Nullable<int> YaoJiLX { get; set; }

        [Display(Name = "浸泡时间")]
        public Nullable<double> JinPaoSJ { get; set; }

        [Display(Name = "药剂浓度")]
        public Nullable<double> YaoJiND { get; set; }

        [Display(Name = "清洗前透水率")]
        public Nullable<double> TouShuiL_Low { get; set; }

        [Display(Name = "清洗后透水率")]
        public Nullable<double> TouShuiL_High { get; set; }

        [Display(Name = "单次清洗氯接触值")]
        public Nullable<double> LvJieCZDC { get; set; }
    }
}
