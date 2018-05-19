using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(WN_ParamSettMetadata))]
    public partial class WN_ParamSett
    {

    }

    public class WN_ParamSettMetadata
    {


        [Required]
        [Display(Name = "参数")]
        public string ParamID { get; set; }


        [Display(Name = "上限值")]
        public Nullable<double> High { get; set; }

        [Display(Name = "下限值")]
        public Nullable<double> Low { get; set; }

        [Display(Name = "判定时间（秒）")]
        public Nullable<int> WaitTime { get; set; }
    }
}
