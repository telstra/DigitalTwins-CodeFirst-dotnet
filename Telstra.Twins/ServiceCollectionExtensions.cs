using System;
using System.Collections.Generic;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Telstra.Twins
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddMediatR(assemblies);
        }
    }
}