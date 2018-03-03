using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class KnowlegeService : BaseService<MBR.Models.Knowlege>
    {
        public KnowlegeService() { }
        public KnowlegeService(MBREntities db) { this.db = db; }

    }
}