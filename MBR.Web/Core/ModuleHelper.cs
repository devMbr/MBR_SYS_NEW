using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using MBR.Models;

namespace MBR.Web
{
    /// <summary>
    /// Module数据处理助手类
    /// </summary>
    public class ModuleHelper
    {
        private List<Module> Modules { get; set; }

        public ModuleHelper(List<Module> Modules)
        {
            this.Modules = Modules;
        }

        public string GetModuleHTML(int? ModuleId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in GetChildren(ModuleId))
            {
                if (HasGenDic.ContainsKey(item.ModuleID)) continue;
                HasGenDic.Add(item.ModuleID, item);
                if (HasChild(item.ModuleID))
                {
                    sb.AppendFormat("<li class=\"treeview\"><a href=\"javascript:;\">");
                    if (!string.IsNullOrEmpty(item.Icon))
                        sb.AppendFormat("<i class=\"{0}\"></i>", item.Icon);
                    else
                        sb.AppendFormat("<i class=\"fa fa-circle-o\"></i>");

                    sb.AppendFormat("<span class=\"title\">{0}</span>", item.FullName);
                    sb.AppendFormat("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span></a>");
                    sb.AppendFormat("<ul class=\"treeview-menu\">");
                    sb.AppendFormat(GetModuleHTML(item.ModuleID));
                    sb.AppendFormat("</ul></li>");
                }
                else
                {
                    string Url = item.Location;
                    if (!string.IsNullOrEmpty(Url))
                    {
                        if (Url.IndexOf("?") > -1)
                        {
                            Url = Url + "&";
                        }
                        else
                        {
                            Url = Url + "?";
                        }
                        Url = string.Format("{0}caption={1}&icon={3}&module={2}", Url, item.FullName, item.ModuleID, item.Icon);
                    }
                    else
                    {
                        Url = "#";
                    }
                    if (string.IsNullOrEmpty(item.Target))
                    {
                        sb.AppendFormat("<li><a id=\"_mes_module_{1}\" href=\"{0}\">", Url, item.ModuleID);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a id=\"_mes_module_{1}\" href=\"{0}\" target=\"{2}\">", Url, item.ModuleID, item.Target);
                    }
                    if (!string.IsNullOrEmpty(item.Icon))
                        sb.AppendFormat("<i class=\"{0}\"></i>", item.Icon);
                    else
                        sb.AppendFormat("<i class=\"fa fa-circle-o\"></i>");
                    sb.AppendFormat("<span class=\"title\">{0}</span></a></li>", item.FullName);
                }
            }
            return sb.ToString();
        }

        public string GetAllModuleHTML()
        {
            return GetModuleHTML(null);
        }

        Dictionary<int, Module> HasGenDic = new Dictionary<int, Module>();
        public string GetModuleListHTML()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Modules)
            {
                if (HasGenDic.ContainsKey(item.ModuleID)) continue;
                HasGenDic.Add(item.ModuleID, item);
                if (HasChild(item.ModuleID))
                {
                    sb.AppendFormat("<li class=\"treeview\"><a href=\"javascript:;\">");
                    if (!string.IsNullOrEmpty(item.Icon))
                        sb.AppendFormat("<i class=\"{0}\"></i>", item.Icon);
                    else
                        sb.AppendFormat("<i class=\"fa fa-circle-o\"></i>");

                    sb.AppendFormat("<span class=\"title\">{0}</span>", item.FullName);
                    sb.AppendFormat("<span class=\"pull-right-container\"><i class=\"fa fa-angle-left pull-right\"></i></span></a>");
                    sb.AppendFormat("<ul class=\"treeview-menu\">");
                    sb.AppendFormat(GetModuleHTML(item.ModuleID));
                    sb.AppendFormat("</ul></li>");
                }
                else
                {
                    string Url = item.Location;
                    if (!string.IsNullOrEmpty(Url))
                    {
                        if (Url.IndexOf("?") > -1)
                        {
                            Url = Url + "&";
                        }
                        else
                        {
                            Url = Url + "?";
                        }
                        Url = string.Format("{0}caption={1}&icon={3}&module={2}", Url, item.FullName, item.ModuleID, item.Icon);
                    }
                    else
                    {
                        Url = "#";
                    }
                    if (string.IsNullOrEmpty(item.Target))
                    {
                        sb.AppendFormat("<li><a id=\"_mes_module_{1}\" href=\"{0}\">", Url, item.ModuleID);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a id=\"_mes_module_{1}\" href=\"{0}\" target=\"{2}\">", Url, item.ModuleID, item.Target);
                    }
                    if (!string.IsNullOrEmpty(item.Icon))
                        sb.AppendFormat("<i class=\"{0}\"></i>", item.Icon);
                    else
                        sb.AppendFormat("<i class=\"fa fa-circle-o\"></i>");
                    sb.AppendFormat("<span class=\"title\">{0}</span></a></li>", item.FullName);
                }
            }
            return sb.ToString();
        }

        private bool HasChild(int ModuleId)
        {
            return Modules.Where(m => m.ParentId == ModuleId).Count() > 0;
        }

        private List<Module> GetChildren(int? ModuleId)
        {
            return Modules.Where(m => m.ParentId == ModuleId).ToList();
        }

    }
}
