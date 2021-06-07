using Iti.Backend.Challenge.CrossCutting.IoC;
using Iti.Backend.Challenge.HostApi.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.HostApi
{

    /// <summary>
    /// Provides methods to setup-up start-up application
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {

        /// <summary>
        /// Create a new Startup instance
        /// </summary>
        /// <param name="configuration">Configuration object instance</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration object
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection object instance</param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(opt => GlobalFilters.Configure(opt))
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                    opt.JsonSerializerOptions.WriteIndented = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iti.Backend.Challenge.HostApi", Version = "v1" });
            });

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddApplicationServices(Configuration);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder object instance</param>
        /// <param name="env">IWebHostEnvironment object instance</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iti.Backend.Challenge.HostApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
