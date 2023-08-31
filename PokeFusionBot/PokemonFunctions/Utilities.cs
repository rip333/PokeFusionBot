namespace PokemonFunctions;

public class Utilities
{
    public static string CombineWords(string word1, string word2)
    {
        if (word2 == null) return word1;
        if (word1 == null) return word2;
        int mid1 = word1.Length / 2;
        int mid2 = word2.Length / 2;

        string firstHalf = word1.Substring(0, mid1);
        string secondHalf = word2.Substring(mid2);

        return UppercaseFirstLetter(firstHalf + secondHalf);
    }

    public static string UppercaseFirstLetter(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            return string.Empty;
        }

        return char.ToUpper(word[0]) + word.Substring(1);
    }
}