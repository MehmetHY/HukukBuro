using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public class Belge
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; }

    public string? Aciklama { get; set; }

    public string Url { get; set; }

    public string? Uzanti { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
