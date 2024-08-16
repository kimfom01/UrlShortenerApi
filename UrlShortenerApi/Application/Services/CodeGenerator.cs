using UrlShortenerApi.Application.Contracts;

namespace UrlShortenerApi.Application.Services;

public class CodeGenerator : ICodeGenerator
{
    private readonly Random _random = new();

    public string GenerateUniqueCode()
    {
        var codeChars = new char[ShortLinkSettings.Length];
        var maxValue = ShortLinkSettings.Alphabet.Length;
        
        for (var i = 0; i < ShortLinkSettings.Length; i++)
        {
            var randomIndex = _random.Next(maxValue);

            codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
        }

        var code = new string(codeChars);

        return code;
    }
}