using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public class Gorev
{
    [Key]
    public int Id { get; set; }

    public string Konu { get; set; }

    public Personel? Personel { get; set; }

    public DateTime BitisTarihi { get; set; }

    public string? Aciklama { get; set; }

    public GorevDurumu Durum { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
