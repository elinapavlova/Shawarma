using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Database;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using Services.Contracts;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
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
            
            services.Configure<AppSettingsOptions>(Configuration.GetSection(AppSettingsOptions.AppSettings));
            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.Token));            
            
            var appSettings = Configuration.GetSection(AppSettingsOptions.AppSettings).Get<AppSettingsOptions>();
            var tokenOptions = Configuration.GetSection(TokenOptions.Token).Get<TokenOptions>(); 
            var signingConfiguration = new SigningConfiguration (tokenOptions.Secret);
            
            services.AddSingleton(appSettings);
            services.AddSingleton(signingConfiguration);

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
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = tokenOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = tokenOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingConfiguration.SecurityKey
                    };
                });
            
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "Bearer {authToken}",
                    Description = "JSON Web Token to access resources. Example: Bearer {token}",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
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