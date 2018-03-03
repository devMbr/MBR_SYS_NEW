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
    public class WarningCondController : BaseController
    {

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            WN_WarningCond entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.WN_WarningCond.Include("WN_ParamSett").Where(m => m.WarningCondID == id).FirstOrDefault();
            }
            return View(entity);
        }

        public JsonResult GetList(GridPager pager, int WarningCondID)
        {
            Expression<Func<WN_ParamSett, bool>> predicate = null;

            using (MBREntities db = new MBREntities())
            {

                var query = db.WN_ParamSett.Where(m => m.WarningCondID == WarningCondID).AsQueryable();
                var list = query.ToList();
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Select(m => new { m.ParamSetID, m.ParamID, m.High, m.Low, m.WaitTime })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(WN_WarningCond entity)
        {
            using (MBREntities db = new MBREntities())
            {
                WarningCondService us = new WarningCondService(db);
                if (us.Create(ref errors, entity))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + entity.WarningCondID, Resource.InsertSucceed, Resource.Create, "预警条件设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + entity.WarningCondID, Resource.InsertFail, Resource.Create, "预警条件设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(WN_WarningCond entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    WarningCondService us = new WarningCondService(db);

                    if (us.Edit(ref errors, entity))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + entity.WarningCondID, Resource.EditSucceed, Resource.Edit, "预警条件设置");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + entity.WarningCondID, Resource.EditFail, Resource.Edit, "预警条件设置");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                    }
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                WarningCondService us = new WarningCondService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + id, Resource.DeleteSucceed, Resource.Delete, "预警条件设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningCondID:" + id, Resource.DeleteFail, Resource.Delete, "预警条件设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }

    }
}
