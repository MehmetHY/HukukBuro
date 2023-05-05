using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class KisiBaglantisi
{
    public int Id { get; set; }

    public int KisiId { get; set; }
    public Kisi Kisi { get; set; }

    public int IlgiliKisiId { get; set; }
    public Kisi IlgiliKisi { get; set; }

    public string? Pozisyon { get; set; }
}