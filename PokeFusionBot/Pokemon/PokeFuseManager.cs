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
            return GetPokeFuseFromPokemonIds(new int[]
            {
                PokeData.GetRandomValueFromDictionary(),
                PokeData.GetRandomValueFromDictionary()
            });
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

        int pokemon1id = pokemonIds[0];
        int pokemon2id = pokemonIds[1];

        var baseUrl = "https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers";
        var url1 = $"{baseUrl}/{pokemon1id}.{pokemon2id}.png";
        var url2 = $"{baseUrl}/{pokemon2id}.{pokemon1id}.png";

        return new PokeFuseResponse(pokemon1id, pokemon2id, url1, url2);
    }
}