using System.Text.RegularExpressions;

namespace ETicaretAPI.Infrastructure.Operations

{
    public static class NameOperation
    {
        public static string CharacterRegulatory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "file";


            name = name
                .Replace("ç", "c").Replace("Ç", "C")
                .Replace("ğ", "g").Replace("Ğ", "G")
                .Replace("ı", "i").Replace("İ", "I")
                .Replace("ö", "o").Replace("Ö", "O")
                .Replace("ş", "s").Replace("Ş", "S")
                .Replace("ü", "u").Replace("Ü", "U");


            name = Regex.Replace(name, @"[^a-zA-Z0-9]", "");

            if (string.IsNullOrWhiteSpace(name))
                return "file";

            return name;
        }
    }
}
