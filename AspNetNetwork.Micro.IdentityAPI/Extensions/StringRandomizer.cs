namespace AspNetNetwork.Micro.IdentityAPI.Extensions;

/// <summary>
/// Represents the string randomizer class.
/// </summary>
public static class StringRandomizer
{
    private const string Chars = @"AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789-+=/\:;!@#$%^&*()_";
    private static readonly Random Random = new();
    
    /// <summary>
    /// Randomize the string.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>Returns randomizing string.</returns>
    public static string Randomize(int length = 16)
    {
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}