using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBR.Models;
using System.Linq.Expressions;
using MBR.Web.Services;
using MBR.Web.Models;

namespace MBR.Web.Controllers
{
    public class MBRInfoController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new MBR_Info
            {
                ChangeDate = DateTime.Now
            });
        }

        public ActionResult Edit(int id)
        {
            MBR_Info entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.MBR_Info.Include("MBR_Pool").Where(m => m.InfoID == id).FirstOrDefault();
            }
            return View(entity);
        }

        public JsonResult GetList(GridPager pager)
        {
            Expression<Func<MBR_Info, bool>> predicate = null;
            if (CurrentMBRPoolID > 0)
            {
                predicate = m => m.PoolID == CurrentMBRPoolID;
            }
            using (MBREntities db = new MBREntities())
            {
                MBRInfoService us = new MBRInfoService(db);

                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m => new { m.InfoID, m.PoolID, m.ChangeDate, m.Company, m.Model, m.Title })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(MBR_Info entity)
        {
            using (MBREntities db = new MBREntities())
            {
                MBRInfoService us = new MBRInfoService(db);
                if (us.Create(ref errors, entity))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + entity.InfoID + ",Title:" + entity.Title, Resource.InsertSucceed, Resource.Create, "MBR膜管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + entity.InfoID + ",Title:" + entity.Title, Resource.InsertFail, Resource.Create, "MBR膜管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(MBR_Info entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    MBRInfoService us = new MBRInfoService(db);

                    if (us.Edit(ref errors, entity))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + entity.InfoID + ",Title:" + entity.Title, Resource.EditSucceed, Resource.Edit, "MBR膜管理");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + entity.InfoID + ",Title:" + entity.Title, Resource.EditFail, Resource.Edit, "MBR膜管理");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                    }
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }

        public ActionResult Details(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                MBRInfoService us = new MBRInfoService(db);
                MBR_Info entity = us.GetById(id);
                return View(entity);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                MBRInfoService us = new MBRInfoService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + id, Resource.DeleteSucceed, Resource.Delete, "MBR膜管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "InfoID:" + id, Resource.DeleteFail, Resource.Delete, "MBR膜管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }

    }
}
