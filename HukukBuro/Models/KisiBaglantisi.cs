﻿using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class KisiBaglantisi
{
    [Key]
    public int Id { get; set; }

    public Kisi Kisi { get; set; }

    public Kisi? IlgiliKisi { get; set; }

    public string Pozisyon { get; set; }
}