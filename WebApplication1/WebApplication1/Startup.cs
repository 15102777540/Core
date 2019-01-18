using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Common;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.FileProviders;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppConfigurtaionServices.Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API",
                    Description = "api文档",
                    TermsOfService = "None"
                });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "WebApplication1.xml");
                var xmlPathByModel = Path.Combine(basePath, "Library.Model.xml");
                /*options.IncludeXmlComments(xmlPathByModel);
                //true表示生成控制器描述，包含true的IncludeXmlComments重载应放在最后，或者两句都使用true
                options.IncludeXmlComments(xmlPath, true);
                options.IncludeXmlComments(xmlPath, true);
                options.OperationFilter<AddAuthTokenHeaderParameter>();*/

            });            
            services.AddOptions();
            services.Configure<AppSettingsModel>(this.Configuration.GetSection("AppSettings"));
            services.AddOptions();
            services.Configure<ApplicationConfiguration>(this.Configuration.GetSection("ApplicationConfiguration"));

            InitAppConfig(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("/wwwroot/Views/Index.html");
            app.UseDefaultFiles(defaultFilesOptions);           

            app.UseHttpsRedirection();
            //app.UseMvc();

            //注册路由api地址
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=GetAz}/{id?}");
            });

            //注册路由core页面地址 
            //启用默认文件夹wwwroot
            app.UseStaticFiles(new StaticFileOptions
            {
                //配置除了默认的wwwroot文件中的静态文件以外的文件夹  提供 Web 根目录外的文件  经过此配置以后，就可以访问Views文件下的文件
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Views")),
                RequestPath = "/Views",
            });
            //启用Swagger调试api
            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.ShowExtensions();
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="services"></param>
        private void InitAppConfig(IServiceCollection services)
        {
            /*var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("Datas/Config/SiteConfig.json")
                .AddJsonFile("Datas/Config/Home/NavBarMenus.json")
                .AddJsonFile("Datas/Config/Home/MyProfile.json")
                .AddJsonFile("Datas/Config/Home/MyRequest.json")
                .AddJsonFile("Datas/Config/BaiduAnalysis/VisitDistrictRequest.json");

            var config = builder.Build();

            services.Configure<SiteConfig>(config.GetSection("SiteConfig"));
            services.Configure<NavBarMenus>(config.GetSection("NavBarMenus"));
            services.Configure<PrivateInfo>(config.GetSection("PrivateInfo"));
            services.Configure<MyProfile>(config.GetSection("MyProfile"));
            services.Configure<MyRequest>(config.GetSection("MyRequest"));
            services.Configure<VisitDistrictRequest>(config.GetSection("VisitDistrictRequest"));*/
        }
    }    

    /// <summary>
    /// 添加Token参数
    /// </summary>
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// 非匿名方法添加Token参数
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
                //非匿名的方法,链接中添加accesstoken值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "token",
                        In = "query",//query header body path formData
                        Type = "string",
                        Required = true //是否必选
                    });
                }
            }
        }
    }
}
