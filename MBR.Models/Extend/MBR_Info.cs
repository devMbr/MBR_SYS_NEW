using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(MBR_InfoMetadata))]
    public partial class MBR_Info
    {

    }

    public class MBR_InfoMetadata
    {

        [Required]
        [Display(Name = "更换时间")]
        public Nullable<System.DateTime> ChangeDate { get; set; }

        [Required]
        [Display(Name = "厂家")]
        [StringLength(100)]
        public string Company { get; set; }

        [Required]
        [Display(Name = "型号")]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "简称")]
        [StringLength(50)]
        public string Title { get; set; }
    }
}
