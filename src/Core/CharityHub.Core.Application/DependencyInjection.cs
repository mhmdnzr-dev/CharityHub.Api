using CharityHub.Core.Contract.Primitives.Handlers;


using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Application;

using System.Reflection;
using Contract.Primitives.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;


public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}