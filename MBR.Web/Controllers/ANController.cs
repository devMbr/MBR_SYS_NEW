using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBR.Models;
using MBR.Web.Models;

namespace MBR.Web.Controllers
{
    public class ANController : BaseController
    {

        public ActionResult Index()
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            foreach (string key in Request.QueryString)
            {
                routeValues.Add(key, Request.QueryString[key]);
            }
            if (!routeValues.ContainsKey("type"))
            {
                routeValues.Add("type", "an");
            }
            return RedirectToAction("Index", "MBRInfo", routeValues);
        }

        public ViewResult ANIndex(int? InfoID)
        {
            MBR_Info Info = null;
            if (InfoID.HasValue)
            {
                using (MBREntities db = new MBREntities())
                {
                    Info = db.MBR_Info.Find(InfoID.Value);
                }
            }
            if (Info == null) throw new Exception("请选择膜");

            ViewBag.MBRInfo = Info;

            ANInfoModel model = new ANInfoModel();

            using (MBREntities db = new MBREntities())
            {
                model.CLCurve = db.AN_CLCurve.Find(InfoID.Value);
                model.HighCurve = db.AN_HighCurve.Find(InfoID.Value);
                model.LowCurve = db.AN_LowCurve.Find(InfoID.Value);
            }

            return View(model);
        }

        /// <summary>
        /// 透水率预测
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public JsonResult GetPermeable(int InfoID = 0)
        {

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 累计氯预测
        /// </summary>
        /// <param name="mbrId"></param>
        /// <returns></returns>
        public JsonResult GetChlorinValue(int InfoID = 0)
        {

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

    }
}
