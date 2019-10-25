using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Rdp.Core.Caching;
using Rdp.Core.Dependency;
using Rdp.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Rdp.Web.Framework.Core
{

    /// <summary>
    /// Session细化项，用于确定哪些session需要清理
    /// </summary>
    /// <remarks></remarks>
    public partial class SessionItem
    {
        public string SessionKey;
        public bool ClearWhenLogout;
    }


    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }


    public class HttpContextOld
    {
        public static HttpContext Current
        {
            get
            {
                HttpContext context = IocObjectManager.GetInstance().Resolve<IHttpContextAccessor>().HttpContext;
                return context;
            }
        }
    }



    /// <summary>
    /// Session控制表
    /// </summary>
    [Serializable()]
    public partial class SessionManager
    {
        private const string RoleUser = "RoleUser";
        private const string UserMaster = "UserMaster";
        private const string Language = "Language";
        private const string Lock = "Lock";

        private const string CrmDomain = "CrmDomain";
        private const string UserInfo = "COU.UserInfo";
        /// <summary>
        /// Session集合，方便后面扩充 add by LT 2015-09-15
        /// </summary>
        /// <remarks></remarks>

        private static HashSet<SessionItem> SessionItems = new HashSet<SessionItem>();
        /// <summary>
        /// 添加Session项到集合， add by LT 2015-09-15
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public static  void AddSessionItem<T>(SessionItem item, T value)
        {
            SessionItems.Add(item);
            
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Set(item.SessionKey, value);
            }
        }

        /// <summary>
        /// 获取Session项到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public static T GetSessionItem<T>(string sessionKey)
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                return context.Session.Get<T>(sessionKey);
            }

            return default(T);
        }

        /// <summary>
        /// 登出登陆处理需要清理的Session， add by LT 2015-09-15
        /// </summary>
        /// <remarks></remarks>
        public static  void Logout()
        {
            foreach (var item in SessionItems)
            {
                if (item.ClearWhenLogout)
                {
                    HttpContext context = HttpContextOld.Current;
                    if ((context != null) && (context.Session != null))
                    {
                        context.Session.Remove(item.SessionKey);
                    }
                }
            }
            //清空Session CJ
            //IocObjectManager.GetInstance().Resolve<IHttpContextSessionManager>().Clear();
        }



        /// <summary>
        /// 添加RoleUser
        /// </summary>
        /// <param name="model"></param>
        /// <remarks></remarks>
        public static  void AddRoleUser(RoleUser model)
        {
            if (model == null)
                throw new ArgumentException("参数不能为NULL");

            var roleUserList = new List<RoleUser>();
            roleUserList.Add(model);
            AddRoleUsers(roleUserList);
        }

        /// <summary>
        /// 获取用户角色，支持多个
        /// </summary>
        /// <returns></returns>
        public static void AddRoleUsers(List<RoleUser> roleUsers)
        {
            if (roleUsers == null || roleUsers.Count <= 0)
                throw new ArgumentException("参数不能为NULL");

            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Set(RoleUser, roleUsers);
            }
        }


        /// <summary>
        /// 删除RoleUser
        /// </summary>
        /// <remarks></remarks>
        public static  void RemoveRoleUser()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Remove(RoleUser);
            }
        }

        /// <summary>
        /// 获取用户角色，支持多个
        /// </summary>
        /// <returns></returns>
        public static List<RoleUser> GetRoleUsers()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                var roleUsers =  context.Session.Get<List<RoleUser>>(RoleUser);

                if (roleUsers == null)
                    return new List<RoleUser>();

                return roleUsers;
            }
            return new List<RoleUser>();
        }


        /// <summary>
        /// 得到RoleUser
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static RoleUser GetRoleUser()
        {
            var roleUsers = GetRoleUsers();
            return roleUsers.Count > 0 ? roleUsers[0] : null;
        }
        /// <summary>
        /// 添加UserMaster
        /// </summary>
        /// <param name="model"></param>
        /// <remarks></remarks>
        public static  void AddUserMaster(UserMaster model)
        {
            if (model == null)
            {
                throw new ArgumentException("参数不能为NULL");
            }
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Set(UserMaster, model);
            }
        }
        /// <summary>
        /// 删除UserMaster
        /// </summary>
        /// <remarks></remarks>
        public static  void RemoveUserMaster()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Remove(UserMaster);
            }
        }
        /// <summary>
        /// 得到UserMaster
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static UserMaster GetUserMaster()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                return context.Session.Get<UserMaster>(UserMaster);
            }
            return null;
        }
        /// <summary>
        /// 添加Language
        /// </summary>
        /// <param name="lan"></param>
        /// <remarks></remarks>
        public static  void AddLanguage(string lan)
        {
            if (lan == null)
            {
                throw new ArgumentException("参数不能为NULL");
            }
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.SetString(Language, lan);
            }
        }
        /// <summary>
        /// 删除Language
        /// </summary>
        /// <remarks></remarks>
        public static  void RemoveLanguage()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Remove(Language);
            }
        }
        /// <summary>
        /// 得到Language
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetLanguage()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                return (string)context.Session.GetString(Language);
            }
            return null;
        }
        /// <summary>
        /// 添加锁定窗口
        /// </summary>
        /// <param name="isLock"></param>
        /// <remarks></remarks>
        public static  void AddLock(string isLock)
        {
            if (isLock == null)
            {
                throw new ArgumentException("参数不能为NULL");
            }
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.SetString(Lock, isLock);
            }
        }
        /// <summary>
        /// 删除锁定窗口
        /// </summary>
        /// <remarks></remarks>
        public static  void RemoveLock()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Remove(Lock);
            }
        }
        /// <summary>
        ///是否锁定窗口
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        /// //todo
        public static string GetLock()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                return context.Session.GetString(Lock);
            }
            return "";
        }
        /// <summary>
        /// 添加锁定窗口
        /// </summary>
        /// <param name="domain"></param>
        /// <remarks></remarks>
        public static  void AddDomain(Domain domain)
        {
            if (domain == null)
            {
                throw new ArgumentException("参数不能为NULL");
            }
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Set(CrmDomain, domain);
            }
        }
        /// <summary>
        /// 删除锁定窗口
        /// </summary>
        /// <remarks></remarks>
        public static  void RemoveDomain()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                context.Session.Remove(CrmDomain);
            }
        }
        /// <summary>
        ///是否锁定窗口
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Domain GetDomain()
        {
            HttpContext context = HttpContextOld.Current;
            if ((context != null) && (context.Session != null))
            {
                return context.Session.Get<Domain>(CrmDomain);
            }
            return null;
        }


        public static  void AddUserInfo(DataRow userDataRow)
        {
            if (userDataRow == null)
            {
                throw new ArgumentException("参数不能为NULL");
            }
            var context = HttpContextOld.Current;
            if (context != null && context.Session != null)
            {
                context.Session.Set(UserInfo, userDataRow);
            }
        }

        public static  void RemoveUserInfo(DataRow userDataRow)
        {
            var context = HttpContextOld.Current;
            if (context != null && context.Session != null)
            {
                context.Session.Remove(UserInfo);
            }
        }

        public static DataRow GetUserInfo()
        {
            var context = HttpContextOld.Current;
            if (context != null && context.Session != null)
            {
                return (DataRow)context.Session.Get<DataRow>(UserInfo);
            }
            return null;
        }
    }
}

