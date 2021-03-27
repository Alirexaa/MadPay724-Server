using MadPay724.Common.Helper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Interface;
using MadPay724.Services.Site.Admin.Auth.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using Microsoft.OpenApi.Models;
using MadPay724.Services.Seed.Interface;
using System.Linq;
using System.Collections.Generic;
using MadPay724.Services.Seed.Service;
using MadPay724.Services.Site.Admin.UserServices.Interface;
using MadPay724.Services.Site.Admin.UserServices.Service;
using Microsoft.Extensions.FileProviders;
using System.IO;
using MadPay724.Services.Upload.Interface;
using MadPay724.Services.Upload.Service;
using Microsoft.Extensions.Logging;
using MadPay724.Presentation.Helper.Filters;
using MadPay724.Common.Helper.Interface;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using MadPay724.Data.Dto.Common.ION;
using MadPay724.Presentation.Helpers.Filters;
using Microsoft.AspNetCore.Identity;
using MadPay724.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MadPay724.Presentation
{
    public class Startup
    {
        private readonly int? _httpsPort;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MadpayDbContext>(p=>p.UseSqlServer("Data Source=. ; Initial Catalog=MadPay724Db ; Integrated Security=True; MultipleActiveResultSets=True;  "));
            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
                config.ReturnHttpNotAcceptable = true;
                config.SslPort = _httpsPort;
                config.Filters.Add(typeof(RequireHttpsAttribute));
                //config.Filters.Add(typeof(LinkRewritingFilter));
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            });

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<MadpayDbContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.Authority = "http://localhost:5000";
                    opt.RequireHttpsMetadata = false;
                    opt.ApiName = "MadPay724Api";
                });


            //services.AddControllers();
            services.AddCors();
            //services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUtilities, Utilities>();
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<SeedService>();
            services.AddScoped<IUnitOfWork<MadpayDbContext>, UnitOfWork<MadpayDbContext>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUploadService, UploadService>();
            //services.AddScoped<ILoggerFactory, LoggerFactory>();
            services.AddScoped<LogFilter>();
            services.AddScoped<UserCheckIdFilter>();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //{
            //    opt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //    };

            //});

            

            //services.AddApiVersioning(opt=> {
            //    opt.ApiVersionReader = new MediaTypeApiVersionReader();
            //    opt.AssumeDefaultVersionWhenUnspecified = true;
            //    opt.ReportApiVersions = true;
            //    opt.DefaultApiVersion = new ApiVersion(1, 0);
            //    opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            //});

            services.AddHsts(opt =>
            {
                opt.MaxAge = TimeSpan.FromDays(180);
                opt.IncludeSubDomains = true;
                opt.Preload = true;

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("SiteApiV1", new OpenApiInfo { Title = "Site Api", Version = "v1" });
                c.SwaggerDoc("PayApiV1", new OpenApiInfo { Title = "Pay Api", Contact = new OpenApiContact { Name = "Alireza Baloochi", Email = "Dev.AlirezaBaloochi@gmail.com" }, Description = "Api for online payment", Version = "v1" });
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}.",


                });
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    new OpenApiSecurityScheme{
                //    Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme }

                //    }
                //});  

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedService seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddAppError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                app.UseHsts();
            }

            seeder.SeedUsers();
            app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("SiteApiV1/swagger.json", "Site Api v1");
                c.SwaggerEndpoint("PayApiV1/swagger.json", "Pay Api v1");
            });


            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString("/wwwroot")

            });

            app.UseMvc();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
