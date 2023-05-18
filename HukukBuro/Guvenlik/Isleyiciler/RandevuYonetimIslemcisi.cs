using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class RandevuYonetimIslemcisi : AuthorizationHandler<RandevuYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RandevuYonetimGerekliligi requirement)
    {
        if (context.RandevuYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
