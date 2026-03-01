using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LittleFeed.Domain.Common;

public static class Slugifier
{
    private static readonly Regex NonSlugChars = new(@"[^a-z0-9\s-]", RegexOptions.Compiled);
    private static readonly Regex MultiSpaceHyphen = new(@"[\s-]+", RegexOptions.Compiled);

    public static string Slugify(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var value = text.Trim().ToLowerInvariant();
        value = RemoveDiacritics(value);
        value = NonSlugChars.Replace(value, "");
        value = MultiSpaceHyphen.Replace(value, "-").Trim('-');

        return value;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var ch in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}