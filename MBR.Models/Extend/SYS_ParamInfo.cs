using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(SYS_ParamInfoMetadata))]
    public partial class SYS_ParamInfo
    {

    }

    public class SYS_ParamInfoMetadata
    {
        [Display(Name = "ParamInfoID")]
        public int ParamInfoID { get; set; }

        [Display(Name = "标题")]
        public string ParamName { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "参数标识")]
        public string ParamKey { get; set; }

        [Display(Name = "类型")]
        public Nullable<int> ParamType { get; set; }
    }
}
