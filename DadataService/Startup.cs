using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Contracts;

namespace DadataService
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
            services.AddHttpClient<IValidateAddressService, ValidateAddressService>("Dadata", 
                client =>
                {
                    client.BaseAddress = new Uri(Configuration["DadataApiSettings:BaseAddress"]);
                    
                    client.DefaultRequestHeaders.Add("X-Secret", Configuration["DadataApiSettings:Secret"]);
                    
                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Token", Configuration["DadataApiSettings:Secret"]);
                });
            
            services.AddScoped<IValidateAddressService, ValidateAddressService>();
            services.AddControllers();
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
                    "default",
                    "{controller=Validate}/{action=ValidateAddress}/{json?}");
            });
        }
    }
}