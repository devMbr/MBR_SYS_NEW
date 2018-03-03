using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;

namespace MBR.Web.Services
{
    public class SettingsService : BaseService<MBR.Models.Settings>
    {
        public SettingsService() { }
        public SettingsService(MBREntities db) { this.db = db; }

        /// <summary>
        /// 先删除,再插入
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Create(ref ValidationErrors errors, Settings entity)
        {
            try
            {
                var list = db.Settings.ToList();
                if (list != null) 
                {
                    foreach (var item in list) 
                    {
                        db.Settings.Remove(item);
                    }
                }
                if (Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add("插入失败");
                    return false;
                }

            }
            catch (Exception ex)
            {
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
    }
}