using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Runtime;
using Rdp.Core.Caching;
using Rdp.Web.Framework.Caching;
using Microsoft.AspNetCore.Http;
using Rdp.Core.Dependency;
using StackExchange.Profiling.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Rdp.Core.Security;
using Microsoft.EntityFrameworkCore;


namespace Rdp.Web.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddLocalization();

            services.AddMvc()
                .AddApplicationPart(typeof(Rdp.Web.Framework.Controllers.AccountController).GetTypeInfo().Assembly)
                .AddControllersAsServices()
                .ConfigureApplicationPartManager(manager => {
                    var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                    manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                    manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
                })
                .AddRazorOptions(options => {
                    options.ViewLocationExpanders.Add(new BetterViewEngine());
                })
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            //增加调优工具
            services.AddMiniProfiler().AddEntityFramework(); 

            //添加Session
            services.AddSession();

            //配置服务appsetting
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //配置HttpContext注入服务
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region 业务系统接口注入
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(DbContext), typeof(RdpDbContext));
            services.AddScoped(typeof(IHttpContextCacheManager), typeof(HttpContextCacheManager));
            services.AddScoped(typeof(IHttpContextSessionManager), typeof(HttpContextSessionManager));

            var typeList = new List<Type[]>() {
                typeof(Rdp.Service.DomainService).Assembly.GetTypes()//,
                //typeof(Tmq.Service.WorkRequestService).Assembly.GetTypes()
            };

            foreach(var types in typeList)
            {
                foreach (var t in types)
                {
                    if (!t.IsInterface && t.GetInterfaces().Where(m => m.IsGenericType && m.GetGenericTypeDefinition() == typeof(IService<>)).Count() >= 1)
                        services.AddScoped(t.GetInterfaces()[0], t);
                }
            }

            IocContainerManager.SetInstance(services.BuildServiceProvider());
            IocObjectManager.SetInstance(new IocObjectManager(new IocLifetimeScope()));

            #endregion

            //配置AutoMapper映射对象
            Rdp.Web.Framework.Runtime.Startup.AutoMapperConfig();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache, IOptions<AppSettings> appSettings)
        {

            AppSettings.SetInstance(appSettings.Value);
            DbHelperSql.CouQuery = DesEncrypt.Decrypt(appSettings.Value.DefaultQueryConn);
            DbHelperSql.DefaultQueryConn = DesEncrypt.Decrypt(appSettings.Value.DefaultQueryConn);
            DbHelperSql.CouUpdate =  DesEncrypt.Decrypt(appSettings.Value.DefaultUpdateConn);
            DbHelperSql.DefaultUpdateConn = DesEncrypt.Decrypt(appSettings.Value.DefaultUpdateConn);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMiniProfiler();

            app.UseSession();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-AU"),
                new CultureInfo("en-GB"),
                new CultureInfo("en"),
                new CultureInfo("es-ES"),
                new CultureInfo("es-MX"),
                new CultureInfo("es"),
                new CultureInfo("fr-FR"),
                new CultureInfo("fr"),
                new CultureInfo("zh-TW"),
                new CultureInfo("zh-CN"),
                new CultureInfo("zh")
            };


            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });

            app.UseMvc(routes =>
            {
            

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    
                    );
            });

            
        }
    }

}
