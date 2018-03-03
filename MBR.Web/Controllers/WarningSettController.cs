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
    public class WarningSettController : BaseController
    {

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult WarningSettEdit()
        {
            WarningSettModel model = new WarningSettModel();
            using (MBREntities db = new MBREntities())
            {
                model.Settings = db.Settings.AsQueryable().FirstOrDefault();
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult WarningSettEdit(WarningSettModel entity)
        {

            if (entity == null && entity.Settings == null)
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
            if (!ModelState.IsValid)
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
            using (MBREntities db = new MBREntities())
            {
                SettingsService us = new SettingsService(db);
                if (us.Create(ref errors, entity.Settings))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "SettingsID:" + entity.Settings.SettingsID, Resource.InsertSucceed, Resource.Create, "预警设置管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "SettingsID:" + entity.Settings.SettingsID, Resource.InsertFail, Resource.Create, "预警设置管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }

        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            WN_WarningSett entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.WN_WarningSett.Find(id);
            }
            return View(entity);
        }

        public JsonResult GetList(GridPager pager)
        {
            Expression<Func<WN_WarningSett, bool>> predicate = null;

            using (MBREntities db = new MBREntities())
            {

                var query = db.WN_WarningSett.AsQueryable();
                var list = query.ToList();
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Select(m => new { m.WarningSettID, m.Title, m.WarningLevel, m.WarningInfo })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(WN_WarningSett entity)
        {
            using (MBREntities db = new MBREntities())
            {
                WarningSettService us = new WarningSettService(db);
                if (us.Create(ref errors, entity))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + entity.WarningSettID + ",Title:" + entity.Title, Resource.InsertSucceed, Resource.Create, "预警规则设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + entity.WarningSettID + ",Title:" + entity.Title, Resource.InsertFail, Resource.Create, "预警规则设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(WN_WarningSett entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    WarningSettService us = new WarningSettService(db);

                    if (us.Edit(ref errors, entity))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + entity.WarningSettID + ",Title:" + entity.Title, Resource.EditSucceed, Resource.Edit, "预警规则设置");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + entity.WarningSettID + ",Title:" + entity.Title, Resource.EditFail, Resource.Edit, "预警规则设置");
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
                WarningSettService us = new WarningSettService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + id, Resource.DeleteSucceed, Resource.Delete, "预警规则设置");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WarningSettID:" + id, Resource.DeleteFail, Resource.Delete, "预警规则设置");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }


        public JsonResult GetConditonList(GridPager pager,int WarningSettID=0)
        {
            Expression<Func<WN_WarningCond, bool>> predicate = null;

            using (MBREntities db = new MBREntities())
            {
                var query = db.WN_WarningCond.Where(m => m.WarningSettID == WarningSettID).AsQueryable();
                var list = query.ToList();
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list.Select(m => new { m.WarningCondID, m.WarningCondDesc })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


    }
}
