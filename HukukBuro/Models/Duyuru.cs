﻿using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Models;

#pragma warning disable CS8618

public sealed class Duyuru
{
    public int Id { get; set; }

    public string Konu { get; set; }

    public string Mesaj { get; set; }

    public string? Url { get; set; }

    public DateTime Tarih { get; set; } = DateTime.Now;
}