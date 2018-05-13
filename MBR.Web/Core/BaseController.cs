using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web.Routing;

using System.Text.RegularExpressions;
using MBR.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MBR.Web
{
    //[LogonActionAttributeFilter]
    public class BaseController : Controller
    {
        protected static Dictionary<int, MBR_Pool> MBRPools = new Dictionary<int, MBR_Pool>();
        /// <summary>
        /// 错误信息
        /// </summary>
        protected ValidationErrors errors = new ValidationErrors();

        /// <summary>
        /// 登陆信息
        /// </summary>
        protected virtual User LogonUser
        {
            get
            {
                return new User
                {
                    UserID = 1,
                    UserName = "admin",
                    RealName = "管理员"
                };
                return System.Web.HttpContext.Current.Session[Constants.SESSION_USERID] as User;
            }
            set
            {
                System.Web.HttpContext.Current.Session[Constants.SESSION_USERID] = value;
            }
        }

        /// <summary>
        /// 当前膜池编号
        /// </summary>
        protected virtual int CurrentMBRPoolID
        {
            get
            {
                return CurrentMBRPool.PoolID;
            }
        }
        /// <summary>
        /// 当前膜池
        /// </summary>
        protected virtual MBR_Pool CurrentMBRPool
        {
            get
            {
                if (MBRPools.Count == 0)
                {
                    using (MBREntities db = new MBREntities())
                    {
                        db.MBR_Pool.ToList().ForEach(p => MBRPools.Add(p.PoolID, p));
                    }

                }
                if (MBRPools.Count == 0)
                {
                    throw new Exception("请维护膜池信息");
                }

                object poolID = System.Web.HttpContext.Current.Request[Constants.MBR_POOL_PARAM_NAME];
                if (poolID == null)
                {
                    poolID = this.RouteData.Values[Constants.MBR_POOL_PARAM_NAME];
                }
                if (poolID == null)
                {
                    poolID = System.Web.HttpContext.Current.Session[Constants.MBR_POOL_PARAM_NAME];
                }

                if (poolID != null)
                {
                    int PoolID = 0;
                    int.TryParse(poolID.ToString(), out PoolID);
                    if (PoolID > 0)
                    {
                        return MBRPools[PoolID];
                    }
                }

                return MBRPools.First().Value;
            }
        }

        protected bool IsLogon()
        {
            return this.LogonUser != null;
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        protected virtual int PageSize
        {
            get
            {
                return 15;
            }
        }
        protected virtual int PageIndex
        {
            get
            {
                int page = 0;
                string fpage = this.Request.Form["page"];
                if (!string.IsNullOrEmpty(fpage))
                {
                    page = Convert.ToInt32(fpage);
                    page = page - 1;
                }
                return page;
            }
        }

        protected ContentResult JsonP(string callback, object data)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return this.Content(string.Format("{0}({1})", callback, json));
        }

        /// <summary>
        /// AOP拦截，在Action执行后
        /// </summary>
        /// <param name="filterContext">filter context</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string PoolID = filterContext.RequestContext.HttpContext.Request[Constants.MBR_POOL_PARAM_NAME];
            if (PoolID != null)
            {
                filterContext.RequestContext.HttpContext.Session[Constants.MBR_POOL_PARAM_NAME] = PoolID;
            }

            ViewBag.MBRPools = MBRPools;
            ViewBag.CurrentMBRPool = this.CurrentMBRPool;

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 产生一些视图数据
        /// </summary>
        protected virtual void RenderViewData()
        {
        }

        /// <summary>
        /// 发生异常写Log
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        protected virtual void LogException(Exception exception, WebExceptionContext exceptionContext = null)
        {

        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new VMEJsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding };
        }
        public new JsonResult Json(object data, JsonRequestBehavior jsonRequest)
        {
            return new VMEJsonResult { Data = data, JsonRequestBehavior = jsonRequest };
        }
        public new JsonResult Json(object data)
        {
            return new VMEJsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }

    public class WebExceptionContext
    {
        public string IP { get; set; }
        public string CurrentUrl { get; set; }
        public string RefUrl { get; set; }
        public bool IsAjaxRequest { get; set; }
        public NameValueCollection FormData { get; set; }
        public NameValueCollection QueryData { get; set; }
        public RouteValueDictionary RouteData { get; set; }
    }

    public class VMEJsonResult : JsonResult
    {
        /// <summary>
        /// 重写执行视图
        /// </summary>
        /// <param name="context">上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                IsoDateTimeConverter timejson = new IsoDateTimeConverter
                {
                    DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
                };

                response.Write(JsonConvert.SerializeObject(Data, timejson));
            }
        }

    }
}
