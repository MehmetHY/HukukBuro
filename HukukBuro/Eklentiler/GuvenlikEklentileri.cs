using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HukukBuro.Eklentiler;

public static class GuvenlikEklentileri
{
    public static string? AnaRolGetir(this ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    #region AnaRol
    public static string? AnarolGetir(this AuthorizationHandlerContext context)
        => context.User.Claims
            .FirstOrDefault(c => c.Type == Sabit.AnaRol.Type)?.Value;

    public static bool AdminMi(this AuthorizationHandlerContext context)
        => context.User.HasClaim(Sabit.AnaRol.Type, Sabit.AnaRol.Admin);

    public static bool YoneticiMi(this AuthorizationHandlerContext context)
        => context.User.HasClaim(Sabit.AnaRol.Type, Sabit.AnaRol.Yonetici);
    #endregion

    #region Yetki
    public static bool KisiYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Kisi));

    public static bool DosyaYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Dosya));

    public static bool PersonelYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Personel));

    public static bool GorevYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Gorev));

    public static bool DuyuruYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Duyuru));

    public static bool RolYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Rol));

    public static bool FinansYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Finans));

    public static bool RandevuYoneticisiMi(this AuthorizationHandlerContext context)
        => context.AdminMi() ||
           (context.YoneticiMi() && context.User.IsInRole(Sabit.Yetki.Randevu));
    #endregion
}
