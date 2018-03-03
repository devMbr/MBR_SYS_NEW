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
    public class KnowlegeController : BaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new Knowlege
            {
                UpdateTime = DateTime.Now
            });
        }

        public ActionResult Edit(int id)
        {
            Knowlege entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.Knowlege.Find(id);
            }
            return View(entity);
        }

        public JsonResult GetList(GridPager pager)
        {
            Expression<Func<Knowlege, bool>> predicate = null;
            if(!string.IsNullOrEmpty(Request["search[value]"]))
            {
                string value = Request["search[value]"];
                predicate = m => m.AllContent.Contains(value);
            }

            using (MBREntities db = new MBREntities())
            {
                KnowlegeService us = new KnowlegeService(db);

                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m => new { m.KnowlegeID, m.Title, m.KeyWords, m.Content, m.Author, m.UpdateTime, m.AllContent, m.Summary })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(Knowlege entity)
        {
            if (entity != null)
            {
                string Title = entity.Title;
                string Author = entity.Author;
                string KeyWords = entity.KeyWords;
                string Content = entity.Content;

                entity.AllContent = string.Format("{0}|{1}|{2}|{3}", Title, Author, KeyWords, Content);

                if (Content != null && Content.Length > 1000) 
                {
                    Content = Content.Substring(0, 1000);
                }
                entity.Summary = string.Format("{0} {1} {2} {3}", Title, Author, KeyWords, Content);

            }

            using (MBREntities db = new MBREntities())
            {
                KnowlegeService us = new KnowlegeService(db);
                if (us.Create(ref errors, entity))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + entity.KnowlegeID + ",Title:" + entity.Title, Resource.InsertSucceed, Resource.Create, "专家库管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + entity.KnowlegeID + ",Title:" + entity.Title, Resource.InsertFail, Resource.Create, "专家库管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(Knowlege entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                string Title = entity.Title;
                string Author = entity.Author;
                string KeyWords = entity.KeyWords;
                string Content = entity.Content;

                entity.AllContent = string.Format("{0}|{1}|{2}|{3}", Title, Author, KeyWords, Content);

                if (Content != null && Content.Length > 1000)
                {
                    Content = Content.Substring(0, 1000);
                }
                entity.Summary = string.Format("{0} {1} {2} {3}", Title, Author, KeyWords, Content);

                using (MBREntities db = new MBREntities())
                {
                    KnowlegeService us = new KnowlegeService(db);

                    if (us.Edit(ref errors, entity))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + entity.KnowlegeID + ",Title:" + entity.Title, Resource.EditSucceed, Resource.Edit, "专家库管理");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + entity.KnowlegeID + ",Title:" + entity.Title, Resource.EditFail, Resource.Edit, "专家库管理");
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
                KnowlegeService us = new KnowlegeService(db);
                Knowlege entity = us.GetById(id);
                return View(entity);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                KnowlegeService us = new KnowlegeService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + id, Resource.DeleteSucceed, Resource.Delete, "专家库管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "KnowlegeID:" + id, Resource.DeleteFail, Resource.Delete, "专家库管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }

    }
}
