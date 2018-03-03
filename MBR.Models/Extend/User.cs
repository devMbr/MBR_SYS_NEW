using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MBR.Models
{

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {

        public bool IsAdmin
        {
            get
            {
                return "admin".Equals(UserName, StringComparison.OrdinalIgnoreCase);
            }
        }

        public String RoleIds
        {
            get
            {
                return "";
            }
        }
        
    }

    public class UserMetadata
    {
        [Required]
        [Display(Name = "登录名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string RealName { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
