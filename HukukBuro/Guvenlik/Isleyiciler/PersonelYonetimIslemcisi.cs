using HukukBuro.Eklentiler;
using HukukBuro.Guvenlik.Gereklilikler;
using Microsoft.AspNetCore.Authorization;

namespace HukukBuro.Guvenlik.Isleyiciler;

public class PersonelYonetimIslemcisi : AuthorizationHandler<PersonelYonetimGerekliligi>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PersonelYonetimGerekliligi requirement)
    {
        if (context.PersonelYoneticisiMi())
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}
