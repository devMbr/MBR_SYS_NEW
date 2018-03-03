using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(SettingsMetadata))]
    public partial class Settings
    {

    }

    public class SettingsMetadata
    {
        [Required]
        [Display(Name = "预警天数")]
        public Nullable<int> PreDays { get; set; }

        [Required]
        [Display(Name = "最大透水率")]
        public Nullable<double> MaxValue { get; set; }

        [Required]
        [Display(Name = "最小透水率")]
        public Nullable<double> MinValue { get; set; }
    }
}
