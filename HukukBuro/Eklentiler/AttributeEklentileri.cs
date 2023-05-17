using System.ComponentModel.DataAnnotations;

namespace HukukBuro.Eklentiler;

public static class AttributeEklentileri
{
    public static TAttribute? AttributeGetir<TAttribute, TEnum>(this TEnum deger)
        where TEnum : Enum
        where TAttribute : Attribute
    {
        var type = typeof(TEnum);
        var memInfo = type.GetMember(deger.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(TAttribute), false);

        return attributes.FirstOrDefault() as TAttribute;
    }

    public static string DisplayName<TEnum>(this TEnum deger) where TEnum : Enum
    {
        var attr = deger.AttributeGetir<DisplayAttribute, TEnum>();

        return attr == null || attr.Name == null ? deger.ToString() : attr.Name;
    }
}
