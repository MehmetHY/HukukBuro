using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class DosyaYonetimIslemcisi : AuthorizationHandler<DosyaYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DosyaYonetimGerekliligi requirement)
    {
        if (context.DosyaYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
