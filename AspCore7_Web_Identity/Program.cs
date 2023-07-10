using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspCore7_Web_Identity.Areas.Identity.Data;
using SignalRMiddleawre.Hubs;
using Service.SQLHelper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Service.IService;
using Service.Service;

namespace AspCore7_Web_Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.WebHost.UseUrls("http://0.0.0.0:5031");
            var connectionString = builder.Configuration.GetConnectionString("SqlDataContextConnection") ?? throw new InvalidOperationException("Connection string 'SqlDataContextConnection' not found.");

            builder.Services.AddDbContext<SqlDataContext>(
                options => options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions => {
                    sqlOptions.EnableRetryOnFailure();
                })
            );
            

            builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SqlDataContext>();

            // Add services to the container.
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            //builder.Services.RegisterSqlHelper(connectionString);
            builder.Services.AddSingleton<ISqlHelperService, SqlHelperService>(sp => new SqlHelperService(connectionString));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSignalR(hubOptions => {
                //hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(7);
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(1);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.MapHub<MainHub>("/mainhub");

            app.Run();
        }
    }
}