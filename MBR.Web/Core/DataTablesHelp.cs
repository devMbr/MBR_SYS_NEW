using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;

namespace MBR.Web
{
    /// <summary>
    /// DataTables数据处理助手类
    /// </summary>
    public static class DataTablesHelp
    {
        public static JsonResult GetJson<T>(this IList<T> datas, int start, int length, JsonRequestBehavior behavior, params string[] fields)
        {
            return GetJson<T>(datas.AsQueryable<T>(), start, length, behavior, fields);
        }

        public static JsonResult GetJson<T>(this IQueryable<T> queriable, int start, int length, JsonRequestBehavior behavior, params string[] fields)
        {
            int draw = 1;
            string sDraw = System.Web.HttpContext.Current.Request["draw"];
            int.TryParse(sDraw, out draw);
            return GetJson<T>(queriable, draw, start, length, behavior, fields);
        }

        public static JsonResult GetJson<T>(this IQueryable<T> queriable, int draw, int start, int length, JsonRequestBehavior behavior, params string[] fields)
        {
            var data = queriable.GetQueryable<T>(start, length);

            var json = new JsonResult();
            json.JsonRequestBehavior = behavior;

            if (length != 0)
            {
                var totalCount = queriable.Count();
                json.Data = new
                {
                    draw = draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = data
                };
            }

            return json;
        }

        private static object[] GetJsonData<T>(IQueryable<T> queriable, params string[] fields)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            T[] datas = queriable.ToArray<T>();

            object[] result = new object[datas.Length];

            if (fields.Length == 0)
            {
                fields = Array.ConvertAll<PropertyInfo, string>(properties.Where<PropertyInfo>
                    (x => x.GetCustomAttributes(typeof(InternalAttribute), false).Length == 0)
                    .ToArray<PropertyInfo>()
                    , delegate(PropertyInfo p)
                    {
                        return p.Name;
                    });
            }

            for (int i = 0; i < datas.Length; i++)
            {
                object[] values = new object[fields.Length];
                for (int j = 0; j < fields.Length; j++)
                {
                    var pi = properties.First<PropertyInfo>(x => x.Name == fields[j]);
                    var value = pi.GetValue(datas[i], null);
                    values[j] = value != null ? value.ToString() : "";
                }

                result[i] = new { id = i, cell = values };
            }

            return result;
        }
    }

}
