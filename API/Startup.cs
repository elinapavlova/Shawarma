using System;
using AutoMapper;
using Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Repository;
using Infrastructure.SwaggerConfiguration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services;
using Services.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API
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
            services.AddHttpClient<IExportActualOrdersToExcelService, ExportActualOrdersToExcelService>("Excel", 
                client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseAddress:ExcelUri"]);
            });
            services.AddHttpClient<IExportActualOrdersToExcelService, ExportActualOrdersToExcelService>("Dadata", 
                client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseAddress:DadataUri"]);
            });

            services.Configure<AppSettingsOptions>(Configuration.GetSection(AppSettingsOptions.AppSettings));
            var appSettings = Configuration.GetSection(AppSettingsOptions.AppSettings).Get<AppSettingsOptions>();
            services.AddSingleton(appSettings);
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IShawarmaService, ShawarmaService>();
            services.AddScoped<IShawarmaRepository, ShawarmaRepository>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IOrderShawarmaService, OrderShawarmaService>();
            services.AddScoped<IOrderShawarmaRepository, OrderShawarmaRepository>();
            
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IExportActualOrdersToExcelService, ExportActualOrdersToExcelService>();
            services.AddScoped<IImportShawarmaFromExcelService, ImportShawarmaFromExcelService>();

            services.AddControllers();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApiContext>(options => options.UseNpgsql(connection,  
                x => x.MigrationsAssembly("Database")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/api/Auth/Login");
                    options.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/api/Auth/Logout");
                    options.Cookie.HttpOnly = true;
                });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader("v");
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    opt => {
                        foreach (var description in provider.ApiVersionDescriptions) {
                            opt.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}