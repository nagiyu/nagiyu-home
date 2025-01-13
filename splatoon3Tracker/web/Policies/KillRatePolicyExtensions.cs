using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nagiyu.Common.Auth.Service.Models;
using Nagiyu.Common.Auth.Service.Services;
using Nagiyu.Splatoon3Tracker.Service.Consts;

namespace Nagiyu.Splatoon3Tracker.Web.Policies
{
    public static class KillRatePolicyExtensions
    {
        public static void AddKillRatePolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(Splatoon3Consts.KILL_RATE_POLICY, policy =>
                policy.RequireAssertion(async context =>
                {
                    if (context.Resource is not HttpContext httpContext)
                    {
                        return false;
                    }

                    var authService = httpContext.RequestServices.GetRequiredService<AuthService>();

                    var user = await authService.GetUser<UserAuthBase>();

                    return user != null;
                })
            );
        }
    }
}
