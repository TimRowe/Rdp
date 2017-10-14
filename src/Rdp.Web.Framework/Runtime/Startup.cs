using System;
using System.Linq;
using Rdp.Core.Dependency;
using Rdp.Data.Entity;
using Rdp.Core.Data;
using Rdp.Web.Framework.Core;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Rdp.Web.Framework.Models;
using Rdp.Service.Dto;
using Rdp.Core.Security;

namespace Rdp.Web.Framework.Runtime
{
    public class Startup
    {
        /*public static void Excute(Action<ContainerBuilder> IocRegisterFun)
        {
            var builder = new ContainerBuilder();
            var baseType = typeof(IDependency);
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList().Where(t => { return t.FullName.Contains("Controllers") | t.FullName.Contains("Filters") | t.FullName.Contains("Models") |   t.FullName.Contains("Rdp") ; });
            builder.RegisterControllers(assemblys.ToArray());
            builder.RegisterFilterProvider();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => baseType.IsAssignableFrom(t) & t != baseType).AsImplementedInterfaces().InstancePerLifetimeScope();
            IocRegisterFun(builder);

            //注入Cache
            //builder.RegisterGeneric(typeof(HttpContextCacheManager)).As(typeof(IHttpContextCacheManager)).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(HttpContextSessionManager)).As(typeof(IHttpContextSessionManager)).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(HttpContextSessionManager)).As(typeof(IHttpContextCacheManager)).InstancePerLifetimeScope();


            //设置相关全局变量
            IocContainerManager.SetInstance(builder.Build());
            IocObjectManager.SetInstance(new IocObjectManager(new IocLifetimeScope()));
            DependencyResolver.SetResolver(new AutofacDependencyResolver(IocContainerManager.GetInstance()));

            //RazorViewEngine设置
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RdpRazorViewEngine());

            AutoMapperConfig();
            RouteConfig();
        }*/

        

        /// <summary>
        /// 路由设置
        /// </summary>
        /*public static void RouteConfig()
        {
            RouteTable.Routes.MapRoute("Default2", "HttpCombiner", new { controller = "HttpCombiner", action = "Index", id = UrlParameter.Optional });
        }*/

        /// <summary>
        /// 设置模型转换
        /// </summary>
        public static void AutoMapperConfig()
        {

            Func<GridParams, string> fun = (GridParams gridParams) =>
            {
                var fieldList = "";
                if (gridParams.Columns != null)
                {
                    gridParams.Columns.ForEach(p => fieldList += p.Field + ",");
                    fieldList = fieldList.Substring(0, fieldList.Length - 1);
                }
                else
                {
                    fieldList = "*";
                }
                return fieldList;
            };

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<short, int>();
                cfg.CreateMap<int, short>();
                cfg.CreateMap<byte, int>();
                cfg.CreateMap<UserMaster, OperateUserMasterModel>().ForMember(d => d.Password, opt => { opt.MapFrom(s => DesEncrypt.Decrypt(s.Password)); });
                cfg.CreateMap<OperateUserMasterModel, UserMaster>().ForMember(d => d.Password, opt => { opt.MapFrom(s => DesEncrypt.Encrypt(s.Password)); });
                cfg.CreateMap<UserMasterSearchModel, UserMasterSearchDto>();
                cfg.CreateMap<RoleMaster, OperateRoleMasterModel>();
                cfg.CreateMap<OperateRoleMasterModel, RoleMaster>();
                cfg.CreateMap<Program, ProgramAddModel>();
                cfg.CreateMap<ProgramAddModel, Program>();
                cfg.CreateMap<ButtonAddModel, Button>();
                cfg.CreateMap<Button, ButtonAddModel>();
                cfg.CreateMap<ProgramButton, ProgramButtonAddModel>();
                cfg.CreateMap<ProgramButtonAddModel, ProgramButton>();
                cfg.CreateMap<RoleUser, RoleUserAddModel>();
                cfg.CreateMap<RoleUserAddModel, RoleUser>();
                cfg.CreateMap<CodeTableItemDto, CommonItemModel>();
                cfg.CreateMap<GridParams, Rdp.Core.Data.PageParam>()
                .ForMember(d => d.FieldList, opt => { opt.MapFrom(s => fun(s)); })
                .ForMember(d => d.pageSize, opt => { opt.MapFrom(s => s.PageSize); })
                .ForMember(d => d.pageIndex, opt => { opt.MapFrom(s => s.PageIndex); })
                .ForMember(d => d.TotalCount, opt => { opt.MapFrom(s => s.TotalCount); })
                .ForMember(d => d.Order, opt => { opt.MapFrom(s => s.SortField + " " + s.SortDirection); });
            });
            
        }
    }
}
