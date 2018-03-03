using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using MBR.Models;
using MBR.Web.Services;
using MBR.Web.Models;

namespace MBR.Web.Controllers
{
    public class UserController : BaseController
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
            using (MBREntities db = new MBREntities())
            {
                var Roles = db.Role.ToList();
                ViewBag.Roles = Roles;

                User entity = db.User.Include(m => m.Role).Where(m => m.UserID == id).FirstOrDefault();

                return View(entity);
            }
        }

        public JsonResult GetList(GridPager pager, string queryStr)
        {
            queryStr = Request["search[value]"];

            Expression<Func<User, bool>> predicate = null;
            if (!string.IsNullOrEmpty(queryStr))
            {
                predicate = m => m.UserName.Contains(queryStr) || m.RealName.Contains(queryStr);
            }
            using (MBREntities db = new MBREntities())
            {
                UserService us = new UserService(db);
                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m => new { m.UserID, m.UserName, m.RealName, m.Enabled })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create(User model)
        {
            using (MBREntities db = new MBREntities())
            {
                UserService us = new UserService(db);

                if (us.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + model.UserID + ",UserName:" + model.UserName, Resource.InsertSucceed, Resource.Create, "用户管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + model.UserID + ",UserName:" + model.UserName, Resource.InsertFail, Resource.Create, "用户管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(User model)
        {
            ModelState.Remove("Password");//不验证密码
            if (model != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    UserService us = new UserService(db);
                    string RoleIds = Request["Role.RoleID"];
                    model.Role = new List<Role>();
                    if (!string.IsNullOrEmpty(RoleIds))
                    {
                        foreach (string item in RoleIds.Split(new Char[] { ',' }))
                        {
                            int RoleId = 0;
                            int.TryParse(item, out RoleId);
                            Role Role = new Role { RoleID = RoleId };
                            model.Role.Add(Role);
                        }
                    }
                    if (us.Edit(ref errors, model))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + model.UserID + ",UserName:" + model.UserName, Resource.EditSucceed, Resource.Edit, "用户管理");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + model.UserID + ",UserName:" + model.UserName, Resource.EditFail, Resource.Edit, "用户管理");
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
                UserService us = new UserService(db);
                User entity = us.GetById(id);
                return View(entity);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                UserService us = new UserService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + id, Resource.DeleteSucceed, Resource.Delete, "用户管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + id, Resource.DeleteFail, Resource.Delete, "用户管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }


        public ViewResult ModifyPass()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ModifyPass(LocalPasswordModel model)
        {
            using (MBREntities db = new MBREntities())
            {
                UserService us = new UserService(db);
                if (model != null && ModelState.IsValid)
                {
                    if (us.ModifyPass(ref errors, LogonUser.UserID, model.OldPassword, model.NewPassword))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + LogonUser.UserID, Resource.EditSucceed, Resource.Edit, "修改密码");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "UserID:" + LogonUser.UserID, Resource.EditFail, Resource.Edit, "修改密码");
                        return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                    }
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
                }
            }
        }
    }
}