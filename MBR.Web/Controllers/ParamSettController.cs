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
    public class ParamSettController : BaseController
    {

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create(int WarningCondID)
        {
            WN_ParamSett entity = new WN_ParamSett();
            entity.WarningCondID = WarningCondID;

            return View(entity);
        }

        public ActionResult Edit(int id)
        {
            WN_ParamSett entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.WN_ParamSett.Where(m => m.ParamSetID == id).FirstOrDefault();
            }
            return View(entity);
        }

        [HttpPost]
        public JsonResult Create(WN_ParamSett entity)
        {
            using (MBREntities db = new MBREntities())
            {
                ParamSettService us = new ParamSettService(db);
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
        public JsonResult Edit(WN_ParamSett entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    ParamSettService us = new ParamSettService(db);

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
                ParamSettService us = new ParamSettService(db);
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
