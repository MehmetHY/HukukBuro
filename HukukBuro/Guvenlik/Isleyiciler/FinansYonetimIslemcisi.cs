using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class FinansYonetimIslemcisi : AuthorizationHandler<FinansYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FinansYonetimGerekliligi requirement)
    {
        if (context.FinansYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
