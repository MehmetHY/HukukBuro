using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class TarafTuru
{
    [Key]
    public int Id { get; set; }

    public string Isim { get; set; }
}