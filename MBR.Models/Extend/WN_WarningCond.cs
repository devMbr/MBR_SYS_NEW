using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(WN_WarningCondMetadata))]
    public partial class WN_WarningCond
    {

    }

    public class WN_WarningCondMetadata
    {

        [Required]
        [Display(Name = "条件说明")]
        public string WarningCondDesc { get; set; }
    }
}
