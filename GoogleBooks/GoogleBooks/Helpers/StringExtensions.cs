using System.Text;
using System.Globalization;

namespace GoogleBooks.Helpers
{
    static class StringExtensions
    {
        public static string RemoveDiacritics(this string word)
        {
            if (string.IsNullOrEmpty(word))
                return null;

            word = word.ToLower();
            var normalizedString = word.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var text = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return text;
        }
    }
}
