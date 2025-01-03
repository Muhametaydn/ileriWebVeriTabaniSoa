using System.Globalization;
using System.Text;

namespace ileriWebVeriTabaniSoa.Helpers
{
    public class StringHelper
    {
        public static string NormalizeTurkishCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var normalizedString = input.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Replace("İ", "i").Replace("ı", "i")
                                             .Replace("ç", "c").Replace("Ç", "C")
                                             .Replace("ğ", "g").Replace("Ğ", "G")
                                             .Replace("ü", "u").Replace("Ü", "U")
                                             .Replace("ş", "s").Replace("Ş", "S")
                                             .Replace("ö", "o").Replace("Ö", "O");
        }
    }
}
