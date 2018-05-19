using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class SYS_ParamInfoService : BaseService<MBR.Models.MBR_WashingInfo>
    {
        public SYS_ParamInfoService() { }
        public SYS_ParamInfoService(MBREntities db) { this.db = db; }

    }
}