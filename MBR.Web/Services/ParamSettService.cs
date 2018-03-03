using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class ParamSettService : BaseService<MBR.Models.WN_ParamSett>
    {
        public ParamSettService() { }
        public ParamSettService(MBREntities db) { this.db = db; }

    }
}