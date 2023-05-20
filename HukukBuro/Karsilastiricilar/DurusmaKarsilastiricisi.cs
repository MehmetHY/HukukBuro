using HukukBuro.Models;

namespace HukukBuro.Karsilastiricilar;

public class DurusmaKarsilastiricisi : IComparer<Durusma>
{
    public int Compare(Durusma? x, Durusma? y)
    {
        if (x == null && y == null)
            return 0;

        if (y == null)
            return 1;

        if (x == null)
            return -1;

        if (x.Tarih == y.Tarih)
            return 0;

        if (x.Tarih < DateTime.Now)
            return x.Tarih < y.Tarih ? -1 : 1;

        return x.Tarih > y.Tarih ? -1 : 1;
    }
}
