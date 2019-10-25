using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Web.Framework.Runtime;
using Rdp.Core.Caching;
using Rdp.Web.Framework.Caching;
using Microsoft.AspNetCore.Http;
using Rdp.Core.Dependency;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using Microsoft.Extensions.Options;
using Rdp.Core.Security;
using Microsoft.EntityFrameworkCore;

using System.Text.Encodings.Web;
using System.Text.Unicode;
using Rdp.Service.Implement;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Rdp.Web.Application
{
    /// <summary>
    /// 具体的依赖注入实现提供容器
    /// </summary>
    public class DIProviderImpl : IDIProvider
    {
        private ServiceProvider _serviceProvider;

        public DIProviderImpl(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }



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

            services.AddScoped<Framework.Filters.PemissionAuthorizeAttribute>();

            services.AddControllersWithViews()
                .AddSessionStateTempDataProvider()
                .AddApplicationPart(typeof(Rdp.Web.Framework.Controllers.AccountController).GetTypeInfo().Assembly)
                .AddControllersAsServices()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new BetterViewEngine());
                }).AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                }); ;
           


            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));
            //增加调优工具
            services.AddMiniProfiler().AddEntityFramework(); 

            //添加Session
            services.AddSession();

            //配置服务appsetting
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddControllersWithViews(); /*.AddNewtonsoftJson()*/

            //配置HttpContext注入服务
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region 业务系统接口注入
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(DbContext), typeof(RdpDbContext));
            services.AddScoped(typeof(IService<>), typeof(DefaultService<>));
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
                    if (!t.IsInterface && t.GetInterfaces().Where(m=> m.Name.Contains("IDependency")).Count() >= 1 && !t.Name.Contains("DefaultService"))
                        services.AddScoped(t.GetInterfaces()[0], t);
                }
            }

            IocObjectManager.SetInstance(new IocObjectManager(new DIProviderImpl(services.BuildServiceProvider())));

            #endregion

            //配置AutoMapper映射对象
            Rdp.Web.Framework.Runtime.Startup.AutoMapperConfig(null);
        }

        


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache, IOptions<AppSettings> appSettings)
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    
                    );
            });

            
        }
    }

}
