using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace System
{
    public static class ExtensionMethods
    {
        public static bool IsLessThanOrEqualZero(this int? value)
        {
            return value != null && value <= 0;
        }

        public static bool IsGreaterThanZero(this int? value)
        {
            return value != null && value > 0;
        }

        public static string GetUniqueFileName(this string fileName)
        {
            fileName = string.Join("-", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
            return Path.GetFileNameWithoutExtension(fileName)
                      + "-"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public static string GetUnitName(this string value)
        {
            return $"{value}-{Guid.NewGuid().ToString().Substring(0, 4)}";
        }

        public static string RemoveInvalidCharacters(this string value)
        {
            return string.Join("-", value.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string GetFileExtensionFromContentDisposition(this string contentDisposition)
        {
            var fileName = ContentDispositionHeaderValue.Parse(contentDisposition).FileName;
            return Path.GetExtension(fileName).RemoveInvalidCharacters();
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }

        public static string ToShortDateString(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToShortDateString() : "";
        }

        public static string ToString(this DateTime? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format) : "";
        }

        public static bool NotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool NullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToHtmlDateFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        public static string ToHtmlDateFormat(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd") : "";
        }

        public static string NullableObjectToString(this object value)
        {
            return value == null ? "" : value.ToString();
        }

        public static string Slugify(this string value)
        {
            var s = value.RemoveAccent().ToLower();
            s = Regex.Replace(s, @"[^a-z0-9\s-]", "");                      // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                       // single space
            s = s.Substring(0, s.Length <= 45 ? s.Length : 45).Trim();      // cut and trim
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens
            return s.ToLower();
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
