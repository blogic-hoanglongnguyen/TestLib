using System.Globalization;

namespace BLogic.Helper.Extensions;

public static class StringExtension
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        // Nếu chỉ có 1 ký tự → viết thường
        if (value.Length == 1)
            return value.ToLowerInvariant();

        // Giữ nguyên phần còn lại
        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(value).Replace(" ", "");
    }
}