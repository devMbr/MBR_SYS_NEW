using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MBR.Web
{
    public class LinqHelper
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static IQueryable<T> DataSorting<T>(IQueryable<T> source, string sortExpression, string sortDirection)
        {
            string sortingDir = string.Empty;
            if (string.IsNullOrEmpty(sortDirection))
                sortingDir = "OrderBy";
            else if (sortDirection.ToUpper().Trim() == "ASC")
                sortingDir = "OrderBy";
            else if (sortDirection.ToUpper().Trim() == "DESC")
                sortingDir = "OrderByDescending";
            else
            {
                //默认升序
                sortingDir = "OrderBy";
            }
            //默认主键排序
            if (string.IsNullOrEmpty(sortExpression))
                sortExpression = typeof(T).Name + "ID";
            ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
            PropertyInfo pi = typeof(T).GetProperty(sortExpression);
            //如果未找到改属性,则直接返回
            if (pi == null) return source;
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IQueryable<T> DataPaging<T>(IQueryable<T> source, int start, int length)
        {
            if (length < 1)
                return source;
            if (start < 0)
                return source;
            return source.Skip(start).Take(length);
        }
        /// <summary>
        /// 排序并分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IQueryable<T> SortingAndPaging<T>(IQueryable<T> source, string sortExpression, string sortDirection, int start, int length)
        {
            IQueryable<T> query = DataSorting<T>(source, sortExpression, sortDirection);
            return DataPaging(query, start, length);
        }
    }
}