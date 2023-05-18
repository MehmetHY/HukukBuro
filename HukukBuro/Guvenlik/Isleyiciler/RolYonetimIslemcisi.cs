using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class RolYonetimIslemcisi : AuthorizationHandler<RolYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RolYonetimGerekliligi requirement)
    {
        if (context.RolYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
