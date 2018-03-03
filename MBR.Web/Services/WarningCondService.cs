using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class WarningCondService : BaseService<MBR.Models.WN_WarningCond>
    {
        public WarningCondService() { }
        public WarningCondService(MBREntities db) { this.db = db; }

    }
}