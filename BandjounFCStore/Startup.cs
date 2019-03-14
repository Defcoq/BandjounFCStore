using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandjounFCStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BandjounFCStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration  configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:BandjounFCStoreProducts:ConnectionString"]));

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            // services.AddMvcCore();
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
                app.UseExceptionHandler("/Error");
            }
            //app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: null,
                   template: "{category}/Page{productPage:int}",
                   defaults: new { controller = "Product", action = "List" }
               );
                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");

                //routes.MapRoute(
                //    name: "pagination",
                //    template: "Products/Page{productPage}",
                //    defaults: new { Controller = "Product", action = "List" }); 

                //routes.MapRoute(
                //   name: "default",
                //   template: "{controller=Product}/{action=List}/{id?}");

                //SeedData.EnsurePopulated(app);

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller}/{action}/{id?}",
                //    defaults: new { controller = "Product", action = "List" });


                


            });
            SeedData.EnsurePopulated(app);

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

        }
    }
}
