﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace HukukBuro.ViewModels.Randevular;

#pragma warning disable CS8618

public class SilVM
{
    public int Id { get; set; }

    public string Kisi { get; set; }

    public string Konu { get; set; }

    public string? Aciklama { get; set; }

    public DateTime Tarih { get; set; }

    public bool TamamlandiMi { get; set; }

    public string? Sorumlu { get; set; }
}
