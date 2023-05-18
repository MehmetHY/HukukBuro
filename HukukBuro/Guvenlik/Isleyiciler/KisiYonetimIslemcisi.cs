using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class KisiYonetimIslemcisi : AuthorizationHandler<KisiYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        KisiYonetimGerekliligi requirement)
    {
        if (context.KisiYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
