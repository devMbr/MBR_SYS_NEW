using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class MBRInfoService : BaseService<MBR.Models.MBR_Info>
    {
        public MBRInfoService() { }
        public MBRInfoService(MBREntities db) { this.db = db; }

    }
}