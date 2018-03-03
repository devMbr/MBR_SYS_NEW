using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Configuration;

namespace MBR.Web
{
    public class Constants
    {

        #region session相关

        public static string SESSION_USERID = "GV_UserID";
        public static string SESSION_USERNAME = "GV_UserName";
        public static string SESSION_USERROLE = "GV_UserRole";
        public static string SESSION_USERMODULE = "GV_UserModule";
        public static string SESSION_USERMODULE_HTML = "GV_UserModuleHTML";

        #endregion

        #region 配置相关

        public static string PRODUCT_NAME = "MBR膜在线专家系统";
        public static string PRODUCT_SHORT_NAME = "MBR";
        public static string COMPANY_NAME = "北京工业大学科技研发中心";

        #endregion


        #region 膜池URL参数名

        public static string MBR_POOL_PARAM_NAME = "GV_PoolID";
        
        #endregion

    }

}