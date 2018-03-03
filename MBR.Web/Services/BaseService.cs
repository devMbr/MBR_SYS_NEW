using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using MBR.Models;
using System.Data;
using System.Transactions;

namespace MBR.Web.Services
{
    public abstract class BaseService<T> where T : class,new()
    {
        internal MBREntities db { set; get; }

        public virtual List<T> GetList()
        {
            return db.Set<T>().ToList();
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).ToList();
        }

        public virtual List<T> GetList(ref GridPager pager, Expression<Func<T, bool>> whereLambda)
        {
            IQueryable<T> queryData = null;
            if (whereLambda != null)
            {
                queryData = db.Set<T>().Where(whereLambda);
            }
            else
            {
                queryData = db.Set<T>();
            }
            pager.recordsTotal = queryData.Count();
            pager.recordsFiltered = pager.recordsTotal;

            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.start, pager.length);

            return queryData.ToList();
        }

        public virtual bool Create(ref ValidationErrors errors, T model)
        {
            try
            {
                T entity = GetById(ref errors, model);
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

        public virtual T GetById(ref ValidationErrors errors, T model)
        {
            return model;
        }

        public virtual bool Delete(ref ValidationErrors errors, params object[] keyValues)
        {
            try
            {
                if (Delete(keyValues) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool Delete(ref ValidationErrors errors, string[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        Delete(deleteCollection);
                        if (SaveChanges() == deleteCollection.Length)
                        {
                            transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool Edit(ref ValidationErrors errors, T model)
        {
            try
            {
                T entity = GetById(ref errors, model);

                if (Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add("编辑失败");
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool IsExist(params object[] keyValues)
        {
            return GetById(keyValues) != null;
        }

        public virtual bool Create(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges() > 0;
        }

        public virtual bool Edit(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                return db.SaveChanges() > 0;
            }
            else if (db.Entry<T>(model).State == EntityState.Unchanged)
            {
                db.Entry<T>(model).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {

                }
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public virtual bool Delete(T model)
        {
            db.Set<T>().Remove(model);
            return db.SaveChanges() > 0;
        }

        public virtual int Delete(params object[] keyValues)
        {
            T model = GetById(keyValues);
            if (model != null)
            {
                db.Set<T>().Remove(model);
                return db.SaveChanges();
            }
            return -1;
        }

        public virtual T GetById(params object[] keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public int ExecuteSql(string sql, params object[] parameters)
        {
            return db.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}