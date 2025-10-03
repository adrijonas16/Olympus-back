using Microsoft.Extensions.DependencyInjection;

namespace Olympus.API.Extensions
{
    public static class AuthorizeProgram
    {
        public static IServiceCollection AddDefaultAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }
    }
}
