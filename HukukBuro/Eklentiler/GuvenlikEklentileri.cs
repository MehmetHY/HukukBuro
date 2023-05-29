using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HukukBuro.Eklentiler;

public static class GuvenlikEklentileri
{
    public static string AnaRolGetir(this ClaimsPrincipal personel)
        => personel.Claims.First(c => c.Type == Sabit.AnaRol.Type).Value;

    #region AnaRol
    public static string? AnarolGetir(this AuthorizationHandlerContext context)
        => context.User.Claims
            .FirstOrDefault(c => c.Type == Sabit.AnaRol.Type)?.Value;

    public static bool AdminMi(this AuthorizationHandlerContext context)
        => context.User.HasClaim(Sabit.AnaRol.Type, Sabit.AnaRol.Admin);

    public static bool YoneticiMi(this AuthorizationHandlerContext context)
        => context.User.HasClaim(Sabit.AnaRol.Type, Sabit.AnaRol.Yonetici);

    public static bool AdminMi(this ClaimsPrincipal personel)
        => personel.HasClaim(c =>
            c.Type == Sabit.AnaRol.Type &&
            c.Value == Sabit.AnaRol.Admin);

    public static bool YoneticiMi(this ClaimsPrincipal personel)
        => personel.HasClaim(c =>
            c.Type == Sabit.AnaRol.Type &&
            c.Value == Sabit.AnaRol.Yonetici);
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


    public static bool KisiYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Kisi));

    public static bool DosyaYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Dosya));

    public static bool PersonelYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Personel));

    public static bool GorevYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Gorev));

    public static bool DuyuruYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Duyuru));

    public static bool RolYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Rol));

    public static bool FinansYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Finans));

    public static bool RandevuYoneticisiMi(this ClaimsPrincipal personel)
        => personel.AdminMi() ||
           (personel.YoneticiMi() && personel.IsInRole(Sabit.Yetki.Randevu));

    #endregion
}
