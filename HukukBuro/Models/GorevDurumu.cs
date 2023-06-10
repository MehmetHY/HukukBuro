using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class GorevDurumu
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; }
}