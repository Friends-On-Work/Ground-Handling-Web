using FluentValidation.AspNetCore;
using Ground_Handlng.Abstractions.Identity;
using Ground_Handlng.Abstractions.SkyLight;
using Ground_Handlng.Abstractions.Utility;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Helper;
using Ground_Handlng.DataObjects.Models;
using Ground_Handlng.DataObjects.Models.Operational;
using Ground_Handlng.DataObjects.Models.UserManagment.Identity;
using Ground_Handlng.DataObjects.Services;
using Ground_Handlng.DataObjects.Services.Identity;
using Ground_Handlng.Utilities.Utility;
using Ground_Handlng.Web.Interfaces.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Ground_Handlng.Web
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
            //services.AddMvc();

          
            //services.AddMvc();
            services.AddScoped<Menu>();
            //services.AddMvc(config =>
            //{
            //    config.Filters.Add(typeof(AdmiLteAuthorizationFilter));
            //    config.Filters.Add(typeof(AdmiLteAuthorizationAsyncFilter));
            //});

            //services.AddAutoMapper(x => x.AddProfile(new MappingEntity()));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AdminLTETemplateConnectionString")));

            services.AddSingleton<PhysicalFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                //options.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Home/AccessDenied");
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IETUserManager, ETUserManager>();
            services.AddTransient<IETRoleManager, ETRoleManager>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IImageOperation, ImageOperation>();            

            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
            //Seed a defaut Account (User, Role and previlaege) 
            services.AddScoped<IDbInitializer, DbInitializer>();

            //calture info
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") };
                options.RequestCultureProviders.Clear();
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Unauthorized";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });

            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //	Whenever possible ensure the cache-control HTTP header is set with no-cache, no-store, must-revalidate; and that the pragma HTTP header is set with no-cache 
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default30",
                    new CacheProfile()
                    {
                        Duration = 0,
                        NoStore = true,
                        Location = ResponseCacheLocation.None
                    });
            });


            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            //ADDING JWT MIDELWARE
            services.AddTransient<TokenManagerMiddleware>();
            services.AddTransient<ITokenManager, DataObjects.Services.TokenManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            //swagger config
            //services.AddSwaggerGen(cfg => {
            //    cfg.SwaggerDoc(
            //        "",
            //        new InfoMasterData
            //        {
            //            Version = Configuration["Prefs:MasterApiVersion"],
            //            Title = Configuration["Prefs:ApiTitle"]
            //        });
            //    cfg.SwaggerDoc(
            //        "Operational",
            //        new Info
            //        {
            //            Version = Configuration["Prefs:OperationalApiVersion"],
            //            Title = Configuration["Prefs:AsyncApiTitle"]
            //        });
            //    cfg.SwaggerDoc(
            //       "Report",
            //       new Info
            //       {
            //           Version = Configuration["Prefs:ReportApiVersion"],
            //           Title = Configuration["Prefs:AsyncApiTitle"]
            //       });
            //    cfg.EnableAnnotations();
            //    cfg.ExampleFilters();
            //    cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Edigital.Bioreguladores.Users.Api.xml"));
            //    cfg.TagActionsBy(p => p.HttpMethod); //Group and order by httpMethod.
            //    cfg.DescribeAllEnumsAsStrings(); //show enums names instead enum values.
            //});
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            //app.UseStatusCodePagesWithReExecute("/error/{0}");

            //if (env.IsDevelopment())
            //{
            //    //app.UseBrowserLink();
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseSession();
            //swager End point
            //app.UseSwaggerUI(cfg =>
            //{
            //    cfg.SwaggerEndpoint("/swagger/MasterData/swagger.json", "MasterData Api");
            //    cfg.SwaggerEndpoint("/swagger/Operational/swagger.json", "Operational  Api");
            //    cfg.SwaggerEndpoint("/swagger/Report/swagger.json", "Report  Api");
            //    cfg.RoutePrefix = string.Empty;
            //    cfg.DefaultModelRendering(ModelRendering.Example);
            //});
            //coustom middle ware
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            //Configure to use identity
            app.UseAuthentication();
            //X-Xss enable
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            //use middle ware
            app.UseMiddleware<TokenManagerMiddleware>();
            dbInitializer.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

                routes.MapRoute(
                    "default",
                    "{area}/{controller}/{action}",
                     new { area = "Account", controller = "Account", action = "Login" });

                routes.MapRoute(
                  name: "authentication",
                  template: "{controller=Account}/{action=Login}/{id?}");

                //routes.MapRoute(
                //    name: "api",
                //    template: "api/{controller=Admin}");
            });
        }
    }
}
