using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletManager.Data;
using WalletManager.Services;

namespace WalletManager
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
            services.AddControllersWithViews();

            services.AddDbContext<WalletManagerContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("WalletManagerContext");

                //options.UseSqlite(connectionString);
                options.UseMySQL(connectionString);

            });

            services.AddAuthorization();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.Cookie.Name = "xid";
                option.AccessDeniedPath = "/index";
                option.LoginPath = "/index";
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IChainService, ChainService>();
            services.AddTransient<ICovalentService, CovalentService>();
            services.AddTransient<IWalletAddressService, WalletAddressService>();
            services.AddTransient<ICoinPriceService, CoinPriceService>();

            services.AddHostedService<TimedUpdateCoinPrice>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    defaults: new { controller = "Home", action = "Index" },
                    pattern: "{action}/{id?}");
                endpoints.MapControllerRoute(
                    name: "route",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
