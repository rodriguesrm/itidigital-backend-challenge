using MediatR;
using Iti.Backend.Challenge.Core.Ports;
using Iti.Backend.Challenge.Core.Services;
using Iti.Backend.Challenge.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Iti.Backend.Challenge.Contract.Options;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Iti.Backend.Challenge.CrossCutting.IoC
{

    /// <summary>
    /// Provides dependency injection bootstrap methods
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {

        /// <summary>
        /// Add application service in container
        /// </summary>
        /// <param name="services">Service collection object</param>
        /// <param name="configuration">Configuration collection</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<PasswordOption>(options => configuration.GetSection("Password").Bind(options));

            services.AddScoped<IPasswordValidateService, PasswordValidateService>();
            services.AddScoped<IPasswordValidateConfigurationProvider, PasswordValidateConfigurationProvider>();

            services.AddServicesMediatR();

            return services;

        }

        /// <summary>
        /// Add mediator services 
        /// </summary>
        /// <param name="services">Service collection object</param>
        private static IServiceCollection AddServicesMediatR(this IServiceCollection services)
        {

            List<string> assembliesNames = new List<string>()
            {
                "Iti.Backend.Challenge.WebApi",
                "Iti.Backend.Challenge.Application"
            };


            assembliesNames
                .ForEach(assemblyName =>
                {
                    var assembly = AppDomain.CurrentDomain.Load(assemblyName);
                    services.AddMediatR(assembly);
                });

            return services;

        }

    }
}
