using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class WarningSettService : BaseService<MBR.Models.WN_WarningSett>
    {
        public WarningSettService() { }
        public WarningSettService(MBREntities db) { this.db = db; }

    }
}