namespace Worker.Service.Extensions;

public static class StringExtensions
{
    public static string LimparNome(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Substituir caracteres indesejados por espaços
        var caracteresIndesejados = new[] { '\'', '\"', '\\','/' };

        // Remove caracteres indesejados
        foreach (var c in caracteresIndesejados)
        {
            input = input.Replace(c.ToString(), string.Empty);
        }

        // Remover espaços extras e transformar em formato de slug
        var nomeLimpo = input.Trim();
        return nomeLimpo;
    }

    public static bool StartsWithAnyNumbers(this string input)
    {
        return !string.IsNullOrEmpty(input) && char.IsDigit(input.TrimStart()[0]);
    }
}