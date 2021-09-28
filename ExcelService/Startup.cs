using AutoMapper;
using Database;
using ExcelService.Services.Contracts;
using Infrastructure;
using Infrastructure.Contracts;
using Infrastructure.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Contracts;

namespace ExcelService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IExportActualOrdersToExcelService, ExportActualOrdersToExcelService>();
            services.AddScoped<IImportShawarmaFromExcelService, ImportShawarmaFromExcelService>();
            
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            
            services.AddScoped<IReportOrderService, ReportOrderService>();
            services.AddScoped<IReportOrderRepository, ReportOrderRepository>();
            
            services.AddControllers();
            services.AddHttpClient();
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            var connection = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<Context>(options => options.UseNpgsql(connection));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "Export",
                    "{controller=Excel}/{action=ExportToExcel}/{json?}");
                
                endpoints.MapControllerRoute(
                    "Import",
                    "{controller=Excel}/{action=ImportFromExcel}/{json?}");
            });
        }
    }
}