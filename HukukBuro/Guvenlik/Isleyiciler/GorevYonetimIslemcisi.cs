using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class GorevYonetimIslemcisi
    : AuthorizationHandler<GorevYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        GorevYonetimGerekliligi requirement)
    {
        if (context.GorevYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
