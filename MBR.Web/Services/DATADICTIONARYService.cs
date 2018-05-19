using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;
using System.Linq.Expressions;
using System.Data;

namespace MBR.Web.Services
{
    public class DATADICTIONARYService : BaseService<MBR.Models.Knowlege>
    {
        public DATADICTIONARYService() { }
        public DATADICTIONARYService(MBREntities db) { this.db = db; }

        private static Dictionary<string, Dictionary<string, string>> DicMap = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, string> GetDicValuesByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            code = code.Trim();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (DicMap.ContainsKey(code))
            {
                return DicMap[code];
            }
            else
            {
                using (MBREntities db = new MBREntities())
                {
                    var dicList = db.DATADICTIONARY.Include("DATADICTIONARYDETAIL").Where(m => m.CODE == code).ToList();

                    if (dicList == null || dicList.Count() != 1)
                        return dic;

                    dic = dicList.First().DATADICTIONARYDETAIL.OrderBy(m => m.SORTCODE).OrderBy(m => m.DATADICTIONARYDETAILID).ToDictionary(m => m.CODE, v => v.FULLNAME);
                }
            }
            return dic;
        }

    }
}