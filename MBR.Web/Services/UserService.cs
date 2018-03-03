using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;
using System.Linq.Expressions;
using System.Data;

namespace MBR.Web.Services
{
    public class UserService : BaseService<MBR.Models.User>
    {
        public UserService() {  }
        public UserService(MBREntities db) { this.db = db; }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        internal MBR.Models.User Login(string UserName, string Password)
        {
            using (MBREntities db = new MBREntities())
            {
                //1.调用业务层方法 根据登陆名查询
                var UserList = db.User.Where(u => u.UserName == UserName && u.Enabled.Value).ToList();
                if (UserList.Count() != 1) return null;
                User User = UserList.First();
                //2.判断是否登陆成功
                string MD5pwd = Encrypt.MD5(Password);
                if (string.Equals(User.Password, MD5pwd, StringComparison.OrdinalIgnoreCase))
                    return User;
                return null;
            }
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        internal List<MBR.Models.Module> GetAllMenuList()
        {
            using (MBREntities db = new MBREntities())
            {
                var query = db.Module.Where(m=>m.Enabled.HasValue && m.Enabled.Value) .AsQueryable();
                return query.OrderBy(m => m.OrderBy).ThenBy(m => m.Code).ToList();
            }
        }

        /// <summary>
        /// 通过用户id获取菜单
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        internal List<MBR.Models.Module> GetUserMenuList(int UserID)
        {
            using (MBREntities db = new MBREntities())
            {
                var queryRole = db.Role.AsQueryable();
                var queryModule = db.Module.AsQueryable();
                var queryModulePermission = db.ModulePermission.AsQueryable();

                var query1 = from t1 in queryRole
                             from t2 in t1.User
                             join t3 in queryModulePermission on new { id = t1.RoleID, type = 1 } equals new { id = t3.ObjectID.Value, type = t3.Category.Value }
                             join t4 in queryModule on t3.ModuleID equals t4.ModuleID
                             where t2.UserID == UserID
                             select t4;
                var query2 = from t1 in queryRole
                             from t2 in t1.User
                             join t3 in queryModulePermission on new { id = t2.UserID, type = 2 } equals new { id = t3.ObjectID.Value, type = t3.Category.Value }
                             join t4 in queryModule on t3.ModuleID equals t4.ModuleID
                             where t2.UserID == UserID
                             select t4;

                return query1.Union(query2).OrderBy(m => m.OrderBy).ThenBy(m => m.Code).Distinct().ToList();
            }
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        internal List<MBR.Models.Role> GetUserRoleList(int UserID)
        {
            using (MBREntities db = new MBREntities())
            {
                var queryRole = db.Role.AsQueryable();
                var query = from t1 in queryRole
                            from t2 in t1.User
                            where t2.UserID == UserID
                            select t1;

                return query.ToList();
            }
        }


        public override bool Edit(ref ValidationErrors errors, User model)
        {
            try
            {
                using (MBREntities db = new MBREntities())
                {
                    User entity = db.User.Where(m => m.UserID == model.UserID).FirstOrDefault();
                    if (entity == null)
                    {
                        errors.Add("实体不存在");
                        return false;
                    }
                    //全部删除
                    if (entity.Role != null && entity.Role.Count() > 0)
                    {
                        entity.Role.Clear();
                    }
                    entity.Role = new List<Role>();

                    entity.RealName = model.RealName;
                    entity.Remark = model.Remark;

                    foreach (var item in model.Role)
                    {
                        var Role = db.Role.Find(item.RoleID);
                        if (Role != null) entity.Role.Add(Role);
                    }
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
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        internal bool ModifyPass(ref ValidationErrors errors, int UserID, string OldPassword, string NewPassword)
        {
            try
            {
                using (MBREntities db = new MBREntities())
                {
                    User entity = GetById(UserID);
                    if (entity == null)
                    {
                        errors.Add("实体不存在");
                        return false;
                    }
                    string MD5pwd = Encrypt.MD5(OldPassword);
                    if (string.Equals(entity.Password, MD5pwd, StringComparison.OrdinalIgnoreCase))
                    {
                        entity.Password = Encrypt.MD5(NewPassword);
                        return Edit(entity);
                    }
                    else
                    {
                        errors.Add("旧密码不正确");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }
    }
}