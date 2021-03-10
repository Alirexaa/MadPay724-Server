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

namespace MadPay724.Presentation
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
            services.AddTransient<ISeedService, SeedService>();
            services.AddControllers();
            services.AddCors();
            services.AddScoped<IUnitOfWork<MadpayDbContext>, UnitOfWork<MadpayDbContext>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("SiteApi", new OpenApiInfo { Title = "Site Api", Version = "v1" });
                c.SwaggerDoc("PayApi", new OpenApiInfo { Title = "Pay Api", Contact = new OpenApiContact { Name = "Alireza", Email = "Alireza@gmail.com" }, Description = "Api for online payment", Version = "v1" });
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ISeedService seeder)
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
            }

            //seeder.SeedUsers();
            app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("SiteApi/swagger.json", "Site Api v1");
                c.SwaggerEndpoint("PayApi/swagger.json", "Pay Api v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
