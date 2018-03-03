using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using MBR.Models;
using MBR.Web.Services;
using System.Data.Objects.SqlClient;

namespace MBR.Web.Controllers
{
    public class RunningInfoController : BaseController
    {
        //
        // GET: /RunningInfo/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetList(GridPager pager, string beginDate = null, string endDate = null)
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
                RunningInfoService us = new RunningInfoService(db);
                var query = us.GetList(ref pager, predicate);
                var json = new
                {
                    draw = pager.draw,
                    recordsTotal = pager.recordsFiltered,
                    recordsFiltered = pager.recordsTotal,
                    data = query.Select(m => new { m.UpdateTime, m.ChanShuiLL, m.ChanShuiYL, m.DanChiMCXQL, m.WuNiND, m.RongJieY, m.KuaMoYC, m.TouShuiL, m.QiShuiB, m.ShuiWen, m.WenDuJZTSL })
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllParam()
        {
            List<SYS_ParamInfo> list = new List<SYS_ParamInfo>();

            list.Add(new SYS_ParamInfo()
            {
                ParamName = "ChanShuiLL",
                Title = "产水流量",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "ChanShuiYL",
                Title = "产水压力",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "DanChiMCXQL",
                Title = "单池膜擦洗气量",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "WuNiND",
                Title = "污泥浓度",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "RongJieY",
                Title = "溶解氧",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "KuaMoYC",
                Title = "跨膜压差",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "TouShuiL",
                Title = "透水率",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "QiShuiB",
                Title = "气水比",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "ShuiWen",
                Title = "水温",
                ParamKey = "",
                ParamType = 1,
            });
            list.Add(new SYS_ParamInfo()
            {
                ParamName = "WenDuJZTSL",
                Title = "温度校正透水率",
                ParamKey = "",
                ParamType = 1,
            });


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDataByParam(string beginDate = null, string endDate = null)
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
                var query = db.MBR_RunningInfo.AsQueryable().Where(predicate).ToList();
                var json = query.Select(m => new
                {
                    UpdateTime = GetIntFromTime(m.UpdateTime.Value),
                    m.ChanShuiLL,
                    m.ChanShuiYL,
                    m.DanChiMCXQL,
                    m.WuNiND,
                    m.RongJieY,
                    m.KuaMoYC,
                    m.TouShuiL,
                    m.QiShuiB,
                    m.ShuiWen,
                    m.WenDuJZTSL
                });

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long lLeft = 621355968000000000;
        /// <summary>
        /// 将数字变成时间
        /// </summary>
        /// <param name="ltime"></param>
        /// <returns></returns>
        public string GetTimeFromInt(long ltime)
        {
            long Eticks = (long)(ltime * 10000) + lLeft;
            DateTime dt = new DateTime(Eticks).ToLocalTime();
            return dt.ToString();
        }
        /// <summary>
        /// 将时间变成数字
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public long GetIntFromTime(DateTime dt)
        {
            DateTime dt1 = dt.ToUniversalTime();
            long Sticks = (dt1.Ticks - lLeft) / 10000;
            return Sticks;
        }

    }
}
