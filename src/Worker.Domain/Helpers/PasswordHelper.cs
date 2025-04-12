using System.Security.Cryptography;
using System.Text;
using System;

namespace Worker.Domain.Helpers;

public static class PasswordHelper
{
    public static string RandomPassword(int minSize, bool uperCase = true, bool lowerCase = true, bool requiredDigit = true, bool requireNonAlphanumeric = true, int requiredUniqueChars = 1, string specialCharactersAcept = "._$*#@")
    {
        StringBuilder builder = new StringBuilder();
        if (uperCase)
            builder.Append(RandomString(1, false));

        if (lowerCase)
            builder.Append(RandomString(minSize, true));
        else
            builder.Append(RandomString(minSize, false));

        if (requireNonAlphanumeric)
            builder.Append(RandomSpecialCharacters(requiredUniqueChars <= 0 ? 1 : requiredUniqueChars, specialCharactersAcept));

        if (requiredDigit)
            builder.Append(RandomNumber(1000, 9999));

        return builder.ToString();
    }

    private static int RandomNumber(int min, int max)
    {
        return RandomNumberGenerator.GetInt32(min, max);
    }

    private static string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        char ch;

        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandomDouble() + 65)));
            builder.Append(ch);
        }

        if (lowerCase)
            return builder.ToString().ToLower();

        return builder.ToString();
    }

    private static string RandomSpecialCharacters(int size, string validChars)
    {
        char[] chars = new char[size];

        for (int i = 0; i < size; i++)
        {
            chars[i] = validChars[RandomNumberGenerator.GetInt32(0, validChars.Length)];
        }

        return new string(chars);
    }

    private static double RandomDouble()
    {
        var rng = new RNGCryptoServiceProvider();
        var bytes = new byte[8];
        rng.GetBytes(bytes);
        var ul = BitConverter.ToUInt64(bytes, 0) / (1 << 11);
        return ul / (double)(1UL << 53);
    }
}