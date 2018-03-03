using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBR.Web
{
    public class LogonActionAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase Request = filterContext.Controller.ControllerContext.HttpContext.Request;

            if (filterContext.Controller.ControllerContext.HttpContext.Session[Constants.SESSION_USERID] == null)
            {
                filterContext.Result = new RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}