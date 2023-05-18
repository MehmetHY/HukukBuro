using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class DuyuruYonetimIslemcisi : AuthorizationHandler<DuyuruYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DuyuruYonetimGerekliligi requirement)
    {
        if (context.DuyuruYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
