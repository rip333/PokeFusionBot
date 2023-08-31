using FuzzySearch;
using Images;

namespace Pokemon;
public class PokeFuseManager : IPokeFuseManager
{
    private const string _randomCommand = "pokerandom";

    public PokeFuseResponse? GetFuseFromMessage(string text)
    {
        var splitString = text.Split(" ");
        if (splitString.Length > 2) return null;

        if (splitString.Any(word => word.ToLower() == _randomCommand))
        {
            return PokeFuseResponse.Random();
        }

        var matches = splitString
            .Take(2)
            .Select(word => GetClosestMatch(word.ToLower()))
            .Where(id => id != 0)
            .ToArray();

        return matches.Length == 2 ? GetPokeFuseFromPokemonIds(matches) : null;
    }

    private int GetClosestMatch(string word)
    {
        // Check for an exact match
        if (PokeData.PokemonIdDictionary.TryGetValue(word, out int exactMatchValue))
        {
            return exactMatchValue;
        }

        // If no exact match, use fuzzy search
        int threshold = 1;
        return PokeData.PokemonIdDictionary
            .Where(kvp => Distance.LevenshteinDistance(word, kvp.Key) <= threshold)
            .Select(kvp => kvp.Value)
            .FirstOrDefault();
    }

    public PokeFuseResponse GetPokeFuseFromPokemonIds(int[] pokemonIds)
    {
        if (pokemonIds.Length < 2)
        {
            return new PokeFuseResponse(0, 0, "", "");
        }
        return new PokeFuseResponse(pokemonIds);
    }
}