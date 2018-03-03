using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using MBR.Models;
using MBR.Web.Services;
using System.Data.Objects.SqlClient;
using System.IO;

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

        public FileResult DownLoadExcel(string beginDate = null, string endDate = null)
        {
            //DataTable dt = (DataTable)Session["datatable"];//获取需要导出的datatable数据
            Expression<Func<MBR_RunningInfo, bool>> predicate = null;
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime dBeginDate = Convert.ToDateTime(beginDate);
                DateTime dEndDate = Convert.ToDateTime(endDate);

                predicate = m => m.UpdateTime >= dBeginDate && m.UpdateTime <= dEndDate;
            }
            if (predicate == null)
            {
                DateTime dBeginDate = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
                DateTime dEndDate = DateTime.Now;
                predicate = m => m.UpdateTime >= dBeginDate && m.UpdateTime <= dEndDate;
            }
            List<MBR_RunningInfo> RunningInfoList = new List<MBR_RunningInfo>();
            using (MBREntities db = new MBREntities())
            {
                RunningInfoList = db.MBR_RunningInfo.AsQueryable().Where(predicate).ToList();
            }
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("分钟");
            int rowIndex = 0;
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(rowIndex);
            rowIndex++;
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("产水流量");
            row1.CreateCell(3).SetCellValue("产水压力");
            row1.CreateCell(4).SetCellValue("单池膜擦洗气量");
            row1.CreateCell(5).SetCellValue("污泥浓度");
            row1.CreateCell(6).SetCellValue("溶解氧");
            row1.CreateCell(7).SetCellValue("跨膜压差");
            row1.CreateCell(8).SetCellValue("透水率");
            row1.CreateCell(9).SetCellValue("温度教正透水率");
            row1.CreateCell(10).SetCellValue("气水比");
            row1.CreateCell(11).SetCellValue("水温");

            //将数据逐步写入sheet1各个行

            foreach (var item in RunningInfoList)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(rowIndex);
                rowtemp.CreateCell(1).SetCellValue(item.UpdateTime.ToString());
                rowtemp.CreateCell(2).SetCellValue(item.ChanShuiLL.ToString());
                rowtemp.CreateCell(3).SetCellValue(item.ChanShuiYL.ToString());
                rowtemp.CreateCell(4).SetCellValue(item.DanChiMCXQL.ToString());
                rowtemp.CreateCell(5).SetCellValue(item.WuNiND.ToString());
                rowtemp.CreateCell(6).SetCellValue(item.RongJieY.ToString());
                rowtemp.CreateCell(7).SetCellValue(item.KuaMoYC.ToString());
                rowtemp.CreateCell(8).SetCellValue(item.TouShuiL.ToString());
                rowtemp.CreateCell(9).SetCellValue(item.QiShuiB.ToString());
                rowtemp.CreateCell(10).SetCellValue(item.ShuiWen.ToString());
                rowtemp.CreateCell(11).SetCellValue(item.WenDuJZTSL.ToString());
                rowIndex++;
            }

            var hourList = RunningInfoList.GroupBy(m => m.UpdateTime.Value.ToString("yyyy-MM-dd HH")).Select(m =>
                new
            {
                UpdateTime = m.Key,
                ChanShuiLL = m.Average(p => p.ChanShuiLL),
                ChanShuiYL = m.Average(p => p.ChanShuiYL),
                DanChiMCXQL = m.Average(p => p.DanChiMCXQL),
                WuNiND = m.Average(p => p.WuNiND),
                RongJieY = m.Average(p => p.RongJieY),
                KuaMoYC = m.Average(p => p.KuaMoYC),
                TouShuiL = m.Average(p => p.TouShuiL),
                QiShuiB = m.Average(p => p.QiShuiB),
                ShuiWen = m.Average(p => p.ShuiWen),
                WenDuJZTSL = m.Average(p => p.WenDuJZTSL)
            }).ToList();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet2 = book.CreateSheet("小时");
            rowIndex = 0;
            //给sheet1添加第一行的头部标题
            row1 = sheet2.CreateRow(rowIndex);
            rowIndex++;
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("产水流量");
            row1.CreateCell(3).SetCellValue("产水压力");
            row1.CreateCell(4).SetCellValue("单池膜擦洗气量");
            row1.CreateCell(5).SetCellValue("污泥浓度");
            row1.CreateCell(6).SetCellValue("溶解氧");
            row1.CreateCell(7).SetCellValue("跨膜压差");
            row1.CreateCell(8).SetCellValue("透水率");
            row1.CreateCell(9).SetCellValue("温度教正透水率");
            row1.CreateCell(10).SetCellValue("气水比");
            row1.CreateCell(11).SetCellValue("水温");

            //将数据逐步写入sheet2各个行

            foreach (var item in hourList)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet2.CreateRow(rowIndex);
                rowtemp.CreateCell(1).SetCellValue(item.UpdateTime.ToString());
                rowtemp.CreateCell(2).SetCellValue(item.ChanShuiLL.ToString());
                rowtemp.CreateCell(3).SetCellValue(item.ChanShuiYL.ToString());
                rowtemp.CreateCell(4).SetCellValue(item.DanChiMCXQL.ToString());
                rowtemp.CreateCell(5).SetCellValue(item.WuNiND.ToString());
                rowtemp.CreateCell(6).SetCellValue(item.RongJieY.ToString());
                rowtemp.CreateCell(7).SetCellValue(item.KuaMoYC.ToString());
                rowtemp.CreateCell(8).SetCellValue(item.TouShuiL.ToString());
                rowtemp.CreateCell(9).SetCellValue(item.QiShuiB.ToString());
                rowtemp.CreateCell(10).SetCellValue(item.ShuiWen.ToString());
                rowtemp.CreateCell(11).SetCellValue(item.WenDuJZTSL.ToString());
                rowIndex++;
            }

            var dayList = RunningInfoList.GroupBy(m => m.UpdateTime.Value.ToString("yyyy-MM-dd")).Select(m =>
                new
                {
                    UpdateTime = m.Key,
                    ChanShuiLL = m.Average(p => p.ChanShuiLL),
                    ChanShuiYL = m.Average(p => p.ChanShuiYL),
                    DanChiMCXQL = m.Average(p => p.DanChiMCXQL),
                    WuNiND = m.Average(p => p.WuNiND),
                    RongJieY = m.Average(p => p.RongJieY),
                    KuaMoYC = m.Average(p => p.KuaMoYC),
                    TouShuiL = m.Average(p => p.TouShuiL),
                    QiShuiB = m.Average(p => p.QiShuiB),
                    ShuiWen = m.Average(p => p.ShuiWen),
                    WenDuJZTSL = m.Average(p => p.WenDuJZTSL)
                }).ToList();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet3 = book.CreateSheet("天");
            rowIndex = 0;
            //给sheet3添加第一行的头部标题
            row1 = sheet3.CreateRow(rowIndex);
            rowIndex++;
            //row1.RowStyle.FillBackgroundColor = "";

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("产水流量");
            row1.CreateCell(3).SetCellValue("产水压力");
            row1.CreateCell(4).SetCellValue("单池膜擦洗气量");
            row1.CreateCell(5).SetCellValue("污泥浓度");
            row1.CreateCell(6).SetCellValue("溶解氧");
            row1.CreateCell(7).SetCellValue("跨膜压差");
            row1.CreateCell(8).SetCellValue("透水率");
            row1.CreateCell(9).SetCellValue("温度教正透水率");
            row1.CreateCell(10).SetCellValue("气水比");
            row1.CreateCell(11).SetCellValue("水温");

            //将数据逐步写入sheet3各个行

            foreach (var item in dayList)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet3.CreateRow(rowIndex);
                rowtemp.CreateCell(1).SetCellValue(item.UpdateTime.ToString());
                rowtemp.CreateCell(2).SetCellValue(item.ChanShuiLL.ToString());
                rowtemp.CreateCell(3).SetCellValue(item.ChanShuiYL.ToString());
                rowtemp.CreateCell(4).SetCellValue(item.DanChiMCXQL.ToString());
                rowtemp.CreateCell(5).SetCellValue(item.WuNiND.ToString());
                rowtemp.CreateCell(6).SetCellValue(item.RongJieY.ToString());
                rowtemp.CreateCell(7).SetCellValue(item.KuaMoYC.ToString());
                rowtemp.CreateCell(8).SetCellValue(item.TouShuiL.ToString());
                rowtemp.CreateCell(9).SetCellValue(item.QiShuiB.ToString());
                rowtemp.CreateCell(10).SetCellValue(item.ShuiWen.ToString());
                rowtemp.CreateCell(11).SetCellValue(item.WenDuJZTSL.ToString());
                rowIndex++;
            }

            string strdate = DateTime.Now.ToString("yyyyMMddhhmmss");//获取当前时间
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", strdate + ".xls");
        }

    }
}
