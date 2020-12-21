using ApplicationCore;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace System
{
    public static class ExtensionMethods
    {
        private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

        public static bool IsLessThanOrEqualZero(this int? value)
        {
            return value != null && value <= 0;
        }

        public static bool IsGreaterThanZero(this int? value)
        {
            return value != null && value > 0;
        }

        public static string ToUniqueFileName(this string fileName)
        {
            fileName = string.Join("-", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
            //fileName = fileName.Slugify();
            return Path.GetFileNameWithoutExtension(fileName)
                      + "-"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public static string ToWebFilePath(this string[] pathSegments)
        {
            return $"/{string.Join("/", pathSegments)}";
        }

        public static string ToThumbnailImagePath(this string filename)
        {
            return $"{Path.GetFileNameWithoutExtension(filename)}_{Constants.THUMBNAIL_IMAGE}{Path.GetExtension(filename)}";
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

        public static string ToHtmlDateTimeLocalFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm");
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
            s = s.Trim();      // cut and trim
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens
            return s.ToLower();
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Text.Encoding.ASCII.GetString(bytes);
        }

        public static DateTime ToESTTime(this DateTime value)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        public static DateTime ToESTTime(this DateTimeOffset value)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(value.UtcDateTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        public static long ToTimestamp(this DateTime value)
        {
            return (value.Ticks - 621355968000000000) / 10000; ;
        }

        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);

                var count = 0;
                var characterBuffer = new char[length];

                for (var iter = 0; iter < length; iter++)
                {
                    var i = byteBuffer[iter] % 87;

                    if (i < 10)
                    {
                        characterBuffer[iter] = (char)('0' + i);
                    }
                    else if (i < 36)
                    {
                        characterBuffer[iter] = (char)('A' + i - 10);
                    }
                    else if (i < 62)
                    {
                        characterBuffer[iter] = (char)('a' + i - 36);
                    }
                    else
                    {
                        characterBuffer[iter] = Punctuations[i - 62];
                        count++;
                    }
                }

                if (count >= numberOfNonAlphanumericCharacters)
                {
                    return new string(characterBuffer);
                }

                int j;
                var rand = new Random();

                for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    int k;
                    do
                    {
                        k = rand.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(characterBuffer[k]));

                    characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                }

                return new string(characterBuffer);
            }
        }

        public static string GenerateCode(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static int ToPageNumber(this int offset, int limit)
        {
            if (offset == 0) return 1;

            return (offset + limit) / limit + 1;
        }
    }
}
