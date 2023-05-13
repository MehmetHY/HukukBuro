using Microsoft.EntityFrameworkCore;

namespace HukukBuro.Eklentiler;

public static class QueryableEklentileri
{
    public static async Task<bool> SayfaGecerliMiAsync<T>(
        this IQueryable<T> q,
        int sayfa,
        int sayfaBoyutu)
    {
        if (sayfa < 1 || sayfaBoyutu < 1)
            return false;

        var toplamOge = await q.CountAsync<T>();
        var toplamSayfa = toplamOge / sayfaBoyutu;

        if (toplamSayfa == 0)
            toplamSayfa = 1;

        if (sayfa > toplamSayfa)
            return false;

        return true;
    }

    public static IQueryable<T> SayfaUygula<T>(
        this IQueryable<T> q,
        int sayfa,
        int sayfaBoyutu)
    {
        return q.Skip((sayfa - 1) * sayfaBoyutu).Take(sayfaBoyutu);
    }

    public static async Task<int> ToplamSayfaAsync<T>(this IQueryable<T> q, int sayfaBoyutu)
    {
        int toplamSayfa = await q.CountAsync() / sayfaBoyutu;

        if (toplamSayfa == 0)
            toplamSayfa = 1;

        return toplamSayfa;
    }
}
