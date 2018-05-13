using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq.Expressions;
using MBR.Models;
using MBR.Web.Services;
using System.Data;
using System.Collections.Generic;
using System.Web.Routing;

namespace MBR.Web.Controllers
{
    public class WashingInfoController : BaseController
    {
        private MBREntities db = new MBREntities();

        /// <summary>
        /// 清洗日志及评价入口
        /// 带washinginfo标志跳转到膜的维护界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            foreach (string key in Request.QueryString) 
            {
                routeValues.Add(key, Request.QueryString[key]);
            }
            if (!routeValues.ContainsKey("type"))
            {
                routeValues.Add("type", "washinginfo");
            }
            return RedirectToAction("Index", "MBRInfo", routeValues);
        }

        /// <summary>
        /// 离线数据补录入口
        /// 带offline标志跳转到膜的维护界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_OfflLine()
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            foreach (string key in Request.QueryString)
            {
                routeValues.Add(key, Request.QueryString[key]);
            }
            if (!routeValues.ContainsKey("type"))
            {
                routeValues.Add("type", "offline");
            }
            return RedirectToAction("Index", "MBRInfo", routeValues);
        }

        public ViewResult WashingInfoIndex(int? InfoID)
        {
            //MBR_Info Info = null;
            //if (InfoID.HasValue)
            //{
            //    using (MBREntities db = new MBREntities())
            //    {
            //        Info = db.MBR_Info.Find(InfoID.Value);
            //    }
            //}
            //if (Info == null) throw new Exception("请选择膜");

            //ViewBag.MBRInfo = Info;

            return View();
        }

        public JsonResult GetList(GridPager pager, int? InfoID)
        {
            Expression<Func<MBR_WashingInfo, bool>> predicate = null;
            if (InfoID.HasValue)
            {
                predicate = m => m.InfoID == InfoID.Value;
            }
            using (MBREntities db = new MBREntities())
            {
                WashingInfoService us = new WashingInfoService(db);
                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m =>
                        new
                        {
                            m.WashingInfoID,
                            m.InfoID,
                            m.BeginTime,
                            m.EndTime,
                            m.WashType,
                            m.YaoJiLX,
                            m.JinPaoSJ,
                            m.YaoJiND,
                            m.TouShuiL_Low,
                            m.TouShuiL_High,
                            TouShuiL_HFL = 0,
                            m.LvJieCZDC,
                            LvJieCZLJ = 0
                        })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


        public ViewResult OffLineIndex(int? InfoID)
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

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult GetRunningInfoList(GridPager pager, string beginDate = null, string endDate = null)
        {
            Expression<Func<MBR_RunningInfo, bool>> predicate = null;
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime dBeginDate = Convert.ToDateTime(beginDate);
                DateTime dEndDate = Convert.ToDateTime(endDate);

                predicate = m => m.UpdateTime >= dBeginDate && m.UpdateTime <= dEndDate;
            }
            using (MBREntities db = new MBREntities())
            {
                var queryData = db.MBR_RunningInfo.AsQueryable();
                if (predicate != null)
                {
                    queryData = queryData.Where(predicate);
                }

                var list = queryData.ToList().GroupBy(m => m.UpdateTime.Value.Date).Select(m => new
                {
                    UpdateTime = m.Key.ToString("yyyy-MM-dd"),
                    ChanShuiLL = m.Average(p => p.ChanShuiLL ?? 0.0).ToString("F2"),
                    TouShuiL = m.Average(p => p.TouShuiL ?? 0.0).ToString("F2")
                });
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = list.Count(),
                    recordsFiltered = list.Count(),
                    data = list
                };


                return Json(json, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetOffLineList(GridPager pager, int? InfoID)
        {
            Expression<Func<MBR_WashingInfo, bool>> predicate = null;
            if (InfoID.HasValue)
            {
                predicate = m => m.InfoID == InfoID.Value;
            }
            using (MBREntities db = new MBREntities())
            {
                WashingInfoService us = new WashingInfoService(db);
                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m =>
                        new
                        {
                            m.WashingInfoID,
                            m.InfoID,
                            m.BeginTime,
                            m.EndTime,
                            m.WashType,
                            m.YaoJiLX,
                            m.JinPaoSJ,
                            m.YaoJiND,
                            m.TouShuiL_Low,
                            m.TouShuiL_High,
                            TouShuiL_HFL = 0,
                            m.LvJieCZDC,
                            LvJieCZLJ = 0
                        })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            return View(new MBR_WashingInfo
            {
                EndTime = DateTime.Now
            });
        }

        public ActionResult Edit(int id)
        {
            MBR_WashingInfo entity = null;
            using (MBREntities db = new MBREntities())
            {
                entity = db.MBR_WashingInfo.Include(m => m.MBR_Info).Include(m => m.MBR_Info.MBR_Pool).Where(m => m.WashingInfoID == id).FirstOrDefault();
            }
            return View(entity);
        }

        [HttpPost]
        public JsonResult Create(MBR_WashingInfo entity)
        {
            using (MBREntities db = new MBREntities())
            {
                WashingInfoService us = new WashingInfoService(db);
                if (us.Create(ref errors, entity))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + entity.WashingInfoID , Resource.InsertSucceed, Resource.Create, "离线清洗数据管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + entity.WashingInfoID , Resource.InsertFail, Resource.Create, "离线清洗数据管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult Edit(MBR_WashingInfo entity)
        {
            if (entity != null && ModelState.IsValid)
            {
                using (MBREntities db = new MBREntities())
                {
                    WashingInfoService us = new WashingInfoService(db);

                    if (us.Edit(ref errors, entity))
                    {
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + entity.WashingInfoID , Resource.EditSucceed, Resource.Edit, "离线清洗数据管理");
                        return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                    }
                    else
                    {
                        string ErrorCol = errors.Error;
                        LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + entity.WashingInfoID , Resource.EditFail, Resource.Edit, "离线清洗数据管理");
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
                WashingInfoService us = new WashingInfoService(db);
                MBR_WashingInfo entity = us.GetById(id);
                return View(entity);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (MBREntities db = new MBREntities())
            {
                WashingInfoService us = new WashingInfoService(db);
                if (us.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + id, Resource.DeleteSucceed, Resource.Delete, "离线清洗数据管理");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(LogonUser.RealName, "WashingInfoID:" + id, Resource.DeleteFail, Resource.Delete, "离线清洗数据管理");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}