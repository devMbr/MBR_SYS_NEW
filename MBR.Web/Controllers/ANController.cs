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
        private MBREntities db = new MBREntities();

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
            model.InfoID = (int)InfoID;
            return View(model);
        }

        /// <summary>
        /// 透水率预测
        /// </summary>
        /// <param name="InfoID"></param>
        /// <returns></returns>
        public JsonResult GetPermeable(int InfoID = 0)
        {



            AN_HighCurve hc = new AN_HighCurve();
            AN_LowCurve lc = new AN_LowCurve();

            using (MBREntities db = new MBREntities())
            {

                hc = db.AN_HighCurve.Find(InfoID);
                lc = db.AN_LowCurve.Find(InfoID);
                
            }
            Dictionary<string, List<ANInfoModel>> dic = new Dictionary<string, List<ANInfoModel>>();

            if (hc != null)
            {
                //高的线的3阶系数
                double hc3 = (double)hc.Coeff_LV3;
                double hc2 = (double)hc.Coeff_LV2;
                double hc1 = (double)hc.Coeff_LV1;
                double hcConst = (double)hc.Const;
                //低线的
                double lc3 = (double)lc.Coeff_LV3;
                double lc2 = (double)lc.Coeff_LV2;
                double lc1 = (double)lc.Coeff_LV1;
                double lcConst = (double)lc.Const;

                List<MBR_WashingInfo> wi = db.MBR_WashingInfo.Where(m => m.InfoID == InfoID).OrderBy(m => m.BeginTime).ToList();

                List<ANInfoModel> highList = new List<ANInfoModel>();

                List<ANInfoModel> lowList = new List<ANInfoModel>();
                for (int i = 0; i <  15; i++)
                {
                    ANInfoModel highAM = new ANInfoModel();
                    //高的线的值
                    double highNumber = hc3 * Math.Pow(i, 3) - hc2 * Math.Pow(i, 2) - hc1 * i + hcConst;//(double)(100 - i)/1.5;//threeSquare + Math.Pow(i, 3) / twoSquare+ Math.Pow(i, 2) + primarySide*i + constant;
                    highAM.XValue = i+1;
                    highAM.YValue = highNumber;
                    highList.Add(highAM);

                    ANInfoModel lowAM = new ANInfoModel();
                    //低的线的值
                    double lowNumber = lc3 * Math.Pow(i, 3) + lc2 * Math.Pow(i, 2) - lc1 * i + lcConst;//(double)(100 - i)/1.5;//threeSquare + Math.Pow(i, 3) / twoSquare+ Math.Pow(i, 2) + primarySide*i + constant;
                    lowAM.XValue = i+1;
                    lowAM.YValue = lowNumber;
                    lowList.Add(lowAM);

                    if (lowNumber >= highNumber)
                    {
                        //清洗之后的值如果小于清洗之前的，吧清洗之前的值赋给之后的，并是最后一次
                        highAM.YValue = lowNumber;
                        break;
                    }
                }
                //清洗之后
                dic.Add("after", highList);
                //清洗之前
                dic.Add("befer", lowList);

                List<ANInfoModel> scaHighList = new List<ANInfoModel>();
                List<ANInfoModel> scaLowList = new List<ANInfoModel>();
                for (int i = 0; i < wi.Count; i++)
                {
                    MBR_WashingInfo rc = wi[i];

                    if (rc.TouShuiL_High == null)
                    {
                        continue;
                    }
                    if (rc.TouShuiL_Low == null)
                    {
                        continue;
                    }


                    ANInfoModel highAM = new ANInfoModel();
                    
                    double highNumber = (double)rc.TouShuiL_High;
                    highAM.XValue = i + 1;
                    highAM.YValue = highNumber;
                    scaHighList.Add(highAM);

                    ANInfoModel lowAM = new ANInfoModel();
                    
                    double lowNumber = (double)rc.TouShuiL_Low;
                    lowAM.XValue = i + 1;
                    lowAM.YValue = lowNumber;
                    scaLowList.Add(lowAM);
                }
                //之后
                dic.Add("afterSca", scaHighList);

                //之前
                dic.Add("beferSca", scaLowList);
                
            }


            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 累计氯预测
        /// </summary>
        /// <param name="mbrId"></param>
        /// <returns></returns>
        public JsonResult GetChlorinValue(int InfoID = 0, int clcTarget = 0)
        {
            AN_CLCurve cc = new AN_CLCurve();

            Dictionary<string, Object> dic = new Dictionary<string, Object>();
            using (MBREntities db = new MBREntities())
            {
                cc = db.AN_CLCurve.Find(InfoID);
            }
            if (cc != null)
            {
                //高的线的3阶系数
                double cc3 = (double)cc.Coeff_LV3;
                double cc2 = (double)cc.Coeff_LV2;
                double cc1 = (double)cc.Coeff_LV1;
                double ccConst = (double)cc.Const;
                List<MBR_WashingInfo> wi = db.MBR_WashingInfo.Where(m => m.InfoID == InfoID).OrderBy(m => m.BeginTime).ToList();

                List<ANInfoModel> clcList = new List<ANInfoModel>();
                for (int i = 0; i <  15; i++)
                {
                    ANInfoModel clcAM = new ANInfoModel();
                    //高的线的值
                    //-0.0005* Math.Pow(i, 3) + 0.2979* Math.Pow(i, 2) + 3.1688*i + 1.7863;
                    double clcNumber = cc3 * Math.Pow(i, 3) + cc2 * Math.Pow(i, 2) + cc1 * i + ccConst;//(double)(100 - i)/1.5;//threeSquare + Math.Pow(i, 3) / twoSquare+ Math.Pow(i, 2) + primarySide*i + constant;
                    clcAM.XValue = i+1;
                    clcAM.YValue = clcNumber;
                    clcList.Add(clcAM);

                    //if (clcNumber >= maxChlorine)
                    //{
                        //clcAM.YValue = maxChlorine;
                        //break;
                    //}
                }
                dic.Add("lineList", clcList);

                List<ANInfoModel> sacList = new List<ANInfoModel>();
                for (int i = 0; i < wi.Count; i++)
                {
                    MBR_WashingInfo rc = wi[i];
                    if (rc.LvJieCZDC == null)
                    {
                        continue;
                    }
                    


                    ANInfoModel clcAM = new ANInfoModel();
                    double clcNumber = (double)rc.LvJieCZDC;
                    clcAM.XValue = i + 1;
                    clcAM.YValue = clcNumber;
                    sacList.Add(clcAM);
                }

                dic.Add("scaList", sacList);

                dic.Add("plotLineValue", clcTarget);
            }
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

    }
}
