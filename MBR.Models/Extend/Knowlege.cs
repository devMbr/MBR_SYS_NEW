using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(KnowlegeMetadata))]
    public partial class Knowlege
    {

    }

    public class KnowlegeMetadata
    {
        [Required]
        [Display(Name = "标题")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "关键字")]
        [StringLength(1000)]
        public string KeyWords { get; set; }

        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "作者")]
        [StringLength(10)]
        public string Author { get; set; }

        [Display(Name = "时间")]
        public Nullable<System.DateTime> UpdateTime { get; set; }

        [Display(Name = "文字")]
        public string AllContent { get; set; }

        [Display(Name = "摘要")]
        public string Summary { get; set; }
    }
}
