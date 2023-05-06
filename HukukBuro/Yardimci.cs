namespace HukukBuro;

public static class Yardimci
{
    public static string OkunabilirDosyaBoyutu(long byteBoyut)
    {
        if (byteBoyut >= 1_000_000_000)
            return $"{byteBoyut / 1_000_000_000.0:F1} GB";

        if (byteBoyut >= 1_000_000)
            return $"{byteBoyut / 1_000_000.0:F1} MB";

        if (byteBoyut >= 1_000)
            return $"{byteBoyut / 1_000.0:F1} KB";

        return $"{byteBoyut} B";
    }
}
