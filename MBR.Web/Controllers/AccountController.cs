using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBR.Web.Models;
using System.Drawing;
using System.Drawing.Imaging;
using MBR.Web.Services;
using MBR.Models;

namespace MBR.Web.Controllers
{
    public class AccountController : Controller
    {
        bool HasVCode = false;

        string RememberMeKey = "RememberMe";
        string UserNameKey = "UserName";
        string PasswordKey = "Password";


        public ActionResult LogOn(string returnUrl)
        {
            System.Web.HttpContext.Current.Session.Clear();

            //是否启用验证码
            ViewBag.HasVCode = HasVCode;
            ViewBag.ReturnUrl = returnUrl;

            string IsAuto = Request["IsAuto"] == null ? "" : Request["IsAuto"].ToString();
            string UserName = Request["UserName"] == null ? "" : Request["UserName"].ToString();
            string Password = Request["Password"] == null ? "" : Request["Password"].ToString();

            LogonModel LogonModel = new Models.LogonModel();
            HttpCookie Cookie = Request.Cookies[RememberMeKey];
            if (Cookie != null)
            {
                string RememberMe = Cookie[RememberMeKey];
                LogonModel.RememberMe = RememberMe == "1";
                string CookieUserName = Cookie[UserNameKey];
                string CookiePassword = Cookie[PasswordKey];
                if (RememberMe == "1" && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    
                }
            }
            return View(LogonModel);
        }

        [HttpPost]
        public ActionResult LogOn(LogonModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                #region 验证码验证
                if (HasVCode)
                {
                    if (!string.Equals(model.ValidateCode, Session["__VCode"]))
                    {
                        ModelState.AddModelError("ValidateCode", "验证码错误！");
                        return View(model);
                    }
                }
                #endregion

                UserService us = new UserService();
                MBR.Models.User user = us.Login(model.UserName, model.Password);
                if (user != null)
                {
                    if (model.RememberMe)
                    {
                        //remerber me 
                        HttpCookie Cookie = Request.Cookies[RememberMeKey];
                        if (Cookie == null)
                        {
                            Cookie = new HttpCookie(RememberMeKey);
                            Cookie[RememberMeKey] = model.RememberMe ? "1" : "0";
                            Cookie[UserNameKey] = model.UserName;
                            //Cookie[PasswordKey] = MES.Core.Encrypt.MD5(model.Password);
                            Cookie[PasswordKey] = model.Password;
                            Cookie.Expires = DateTime.Now.AddMonths(1);
                        }
                        else
                        {

                        }
                        Response.SetCookie(Cookie);
                    }

                    #region 权限相关
                    List<Module> MenuList = new List<Module>();
                    
                    if (user.IsAdmin)
                    {
                        MenuList = us.GetAllMenuList();
                    }
                    else
                    {
                        MenuList = us.GetUserMenuList(user.UserID);
                    }
                    #endregion

                    #region 角色相关
                    List<Role> RoleList = new List<Role>();
                    RoleList = us.GetUserRoleList(user.UserID);
                    Role Role = RoleList.FirstOrDefault();
                    #endregion

                    #region 生成菜单
                    ModuleHelper mh = new ModuleHelper(MenuList);
                    string MenuHTML = mh.GetModuleListHTML();
                    #endregion

                    #region  会话
                    System.Web.HttpContext.Current.Session[Constants.SESSION_USERID] = user;
                    System.Web.HttpContext.Current.Session[Constants.SESSION_USERMODULE] = MenuList;
                    System.Web.HttpContext.Current.Session[Constants.SESSION_USERMODULE_HTML] = MenuHTML;
                    #endregion

                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError("", "用户名或密码不正确。");
            return View(model);
        }

        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Session.Clear();
            HttpCookie Cookie = Request.Cookies[RememberMeKey];
            if (Cookie != null)
            {
                Cookie.Expires = new DateTime(1970, 1, 1);
                Response.SetCookie(Cookie);
            }
            return RedirectToAction("LogOn");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        public void ValidateCode()
        {
            string vnum;
            vnum = GetByRndNum(4);
            Response.ClearContent();
            Response.ContentType = "image/jpeg";

            CreateValidateCode(vnum);
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="vnum"></param>
        private void CreateValidateCode(string vnum)
        {
            Bitmap Img = null;
            Graphics g = null;
            Random random = new Random();
            int gheight = vnum.Length * 15;
            Img = new Bitmap(gheight, 26);
            g = Graphics.FromImage(Img);
            Font f = new Font("微软雅黑", 16, FontStyle.Bold);

            g.Clear(Color.White);//设定背景色
            Pen blackPen = new Pen(ColorTranslator.FromHtml("#e1e8f3"), 18);

            for (int i = 0; i < 128; i++)
            {
                int x = random.Next(gheight);
                int y = random.Next(20);
                int xl = random.Next(6);
                int yl = random.Next(2);
                g.DrawLine(blackPen, x, y, x + xl, y + yl);
            }

            SolidBrush s = new SolidBrush(ColorTranslator.FromHtml("#411464"));
            g.DrawString(vnum, f, s, 1, 1);

            //画边框
            blackPen.Width = 1;
            g.DrawRectangle(blackPen, 0, 0, Img.Width - 1, Img.Height - 1);
            Img.Save(Response.OutputStream, ImageFormat.Jpeg);
            s.Dispose();
            f.Dispose();
            blackPen.Dispose();
            g.Dispose();
            Img.Dispose();

            //Response.End();
        }

        //-----------------给定范围获得随机颜色
        Color GetByRandColor(int fc, int bc)
        {
            Random random = new Random();

            if (fc > 255)
                fc = 255;
            if (bc > 255)
                bc = 255;
            int r = fc + random.Next(bc - fc);
            int g = fc + random.Next(bc - fc);
            int b = fc + random.Next(bc - bc);
            Color rs = Color.FromArgb(r, g, b);
            return rs;
        }

        //取随机产生的认证码(数字)
        public string GetByRndNum(int VcodeNum)
        {

            string VNum = "";

            Random rand = new Random();

            for (int i = 0; i < VcodeNum; i++)
            {
                VNum += VcArray[rand.Next(0, 9)];
            }
            Session["__VCode"] = VNum;
            return VNum;
        }

        private static readonly string[] VcArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


    }
}
