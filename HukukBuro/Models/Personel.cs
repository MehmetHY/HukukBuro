using Microsoft.AspNetCore.Identity;

namespace HukukBuro.Models;

public sealed class Personel : IdentityUser
{
    public List<Duyuru> OkunmamisDuyurular { get; set; } = new();
}
