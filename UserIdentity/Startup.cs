﻿using DevExpress.AspNetCore.Bootstrap;
using DevExpress.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserIdentity.Infrastructure;
using UserIdentity.Models;

namespace UserIdentity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidatore>();
            services.AddSingleton<IClaimsTransformation, LocationClaimsProvider>();
            services.AddTransient<IAuthorizationHandler, BlockUsersHandler>();
            services.AddTransient<IAuthorizationHandler, DocumentAuthorizationHandler>();
            services.AddAuthorization(options =>
            {

                options.AddPolicy("DCUsers", policy =>
                {
                    policy.RequireRole("Users");

                    policy.RequireClaim(ClaimTypes.StateOrProvince, "DC");
                });

                options.AddPolicy("NotBob", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new BlockUsersRequirement("Bob"));
                });

                options.AddPolicy("AuthorsAndEditors", policy =>
                {
                    policy.AddRequirements(new DocumentAuthorizationRequirement
                    {
                        AllowAuthors = true,
                        AllowEditors = true
                    });
                });



            });
            services.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = "296694929092-21udo50mte70eki1ks7faab7mleelqfp.apps.googleusercontent.com";
                opts.ClientSecret = "B6KvI8cbuMMPjfTzZQiugj5t";
            });
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(Configuration["Data:UserIdentity:ConnectionString"]));


            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                // options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc();


            services.AddDevExpressControls(options =>
            {
                options.Bootstrap(bootstrapOptions =>
                {
                    bootstrapOptions.IconSet = BootstrapIconSet.Embedded;
                    bootstrapOptions.Mode = BootstrapMode.Bootstrap4;
                });
                options.Resources = ResourcesType.DevExtreme;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            // AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration);

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseDevExpressControls();
        }
    }
}
