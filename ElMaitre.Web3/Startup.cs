using ElMaitre.DAL.Data;
using ElMaitre.DAL.Repositories;
using ElMaitre.Services;
using ElMaitre.Web.Configuration;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.Logger;
using ElMaitre.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace ElMaitre.Web3
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            //loggerFactory.ConfigureNLog(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                   opts =>
                   {
                       var supportedCultures = new List<CultureInfo>
                       {
                            //new CultureInfo("en-US"),
                            //new CultureInfo("en"),
                            new CultureInfo("ar-EG"),
                            new CultureInfo("ar"),
                       };

                       opts.DefaultRequestCulture = new RequestCulture("ar-EG");
                       // Formatting numbers, dates, etc.
                       opts.SupportedCultures = supportedCultures;
                       // UI strings that we have localized.
                       opts.SupportedUICultures = supportedCultures;
                   });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ILawyerService, LawyerService>();
            services.AddTransient<ILawyerAppointmentService, LawyerAppointmentService>();
            services.AddTransient<ISesstionService, SesstionService>();
            services.AddTransient<IPriceRangeService, PriceRangeService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
                config.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-_ ";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddSession();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://elmaitre.com",
                    ValidIssuer = "http://elmaitre.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.SecurityKey))
                };

            });
            //.AddCookie("Cook")
            //.AddGoogle(config =>
            //{
            //    //config.SignInScheme = "Cook";
            //    config.ClientId = Configuration["Authentication:Google:ClientId"];
            //    config.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //    //config.CallbackPath = new PathString("/account/GoogleSignIn");

            //    //config.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "UserId");
            //    //config.ClaimActions.MapJsonKey(ClaimTypes.Email, "EmailAddress", ClaimValueTypes.Email);
            //    //config.ClaimActions.MapJsonKey(ClaimTypes.Name, "Name");

            //});
            //.AddFacebook(config=>
            //{
            //    config.SignInScheme = "Cook";
            //    config.ClientId = Configuration["Authentication:Facebook:AppId"];
            //    config.ClientSecret = Configuration["Authentication:Facebook:AppSecret"];
            //    config.CallbackPath = new PathString("/account/FacebookSignIn");
            //});

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build());
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


        }



        // This method gets called by the runtime. Use this method to configure the http request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerManager logger)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseRewriter(new RewriteOptions().AddRedirectTohttp(StatusCodes.Status301MovedPermanently, 443));
                app.UseExceptionHandler("/Home/Error");
            }


            bool recreateDB = Configuration.GetValue<bool>("AppSettings:ReCreateDB");
            SeedDatabase.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider, recreateDB);
            app.UseAuthentication();

            //app.UseMvc();
            app.UseSession();
            app.UseStaticFiles();
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.ConfigureExceptionHandler(logger);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
