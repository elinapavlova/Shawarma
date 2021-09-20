using System.Linq;
using AutoMapper;
using Database;
using Export.Services;
using Export.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Infrastructure.Contracts;
using Infrastructure.Profiles;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IShawarmaService, ShawarmaService>();
            services.AddScoped<IShawarmaRepository, ShawarmaRepository>();
            
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            
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
        
        public class SwaggerDefaultValues : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var apiDescription = context.ApiDescription;
                operation.Deprecated |= apiDescription.IsDeprecated();

                if (operation.Parameters == null)
                    return;
                
                foreach (var parameter in operation.Parameters)
                {
                    var description = apiDescription.ParameterDescriptions
                        .First(p => p.Name == parameter.Name);
                    
                    if (parameter.Description == null)
                        parameter.Description = description.ModelMetadata?.Description;

                    if (parameter.Schema.Default == null && description.DefaultValue != null)
                        parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

                    parameter.Required |= description.IsRequired;
                }
            }
        }
        
        public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider _provider;
            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
                _provider = provider;
            public void Configure(SwaggerGenOptions options)
            {
                foreach (var description in _provider.ApiVersionDescriptions) {
                    options.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo
                        {
                            Title = $"Sample API {description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                        });
                }
            }
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
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