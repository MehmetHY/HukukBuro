using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class FinansIslemTuru
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; }    // DosyaGeliri | DosyaGideri | OfisGenelGideri | PersonelMaasi | PersonelAvans | Iade | KasaDuzeltme | Transfer | SerbestMeslek | Diger
}