using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(WN_WarningSettMetadata))]
    public partial class WN_WarningSett
    {

    }

    public class WN_WarningSettMetadata
    {

        [Required]
        [Display(Name = "警报标题")]
        public string Title { get; set; }

        [Display(Name = "警报级别")]
        public Nullable<int> WarningLevel { get; set; }

        [Display(Name = "警报信息")]
        public string WarningInfo { get; set; }
    }
}
