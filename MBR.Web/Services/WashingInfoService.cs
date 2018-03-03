using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class WashingInfoService : BaseService<MBR.Models.MBR_WashingInfo>
    {
        public WashingInfoService() { }
        public WashingInfoService(MBREntities db) { this.db = db; }

    }
}