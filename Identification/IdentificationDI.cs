using Identification.Base.Contracts;
using Identification.Core.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Identification.Core;

public static class IdentificationDI
{
    public static IServiceCollection AddIdentificationSupport(this IServiceCollection services)
    {
        services.AddScoped<IIdentityInfo, IdentityInfo>();
        services.AddScoped<IInfoSetter, InfoSetter>();

        return services;
    }
}
