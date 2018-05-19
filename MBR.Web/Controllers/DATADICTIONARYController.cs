using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBR.Web.Models;
using MBR.Models;
using System.Linq.Expressions;
using MBR.Web.Services;

namespace MBR.Web.Controllers
{
    public class DATADICTIONARYController : BaseController
    {


        public JsonResult GetDicValuesByCode(string code)
        {
            DATADICTIONARYService DATADICTIONARYService = new DATADICTIONARYService();

            return Json(DATADICTIONARYService.GetDicValuesByCode(code), JsonRequestBehavior.AllowGet);

        }


    }
}
