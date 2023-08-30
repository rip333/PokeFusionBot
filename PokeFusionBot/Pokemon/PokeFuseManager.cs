using FuzzySearch;

namespace Pokemon;
public class PokeFuseManager
{
    public async static Task<PokeFuseResponse?> GetFuseFromMessage(string text)
    {
        var splitString = text.Split(" ");
        if (splitString.Length > 3) return null;

        Console.WriteLine(text);
        var matches = new int[2];
        int i = 0;
        foreach (var word in splitString)
        {
            if (i == 2) break;

            if (word.ToLower() == "pokerandom") {
                matches[0] = PokeData.GetRandomValueFromDictionary();
                matches[1] = PokeData.GetRandomValueFromDictionary();
                i = 2;
                break;
            }

            int threshold = 1; // This value can be adjusted
            foreach (var key in PokeData.dict.Keys)
            {
                if (i == 2) break;
                if (Distance.LevenshteinDistance(word.ToLower(), key) <= threshold)
                {
                    int id = PokeData.dict[key];
                    if (id != 0)
                    {
                        matches[i] = id;
                        i++;
                    }
                }
            }

        }

        if (matches.Length == 2)
        {
            var url = await GetPokeFuseFromPokemonIds(matches);
            return url;
        }
        return null;
    }

    public static async Task<PokeFuseResponse?> GetPokeFuseFromPokemonIds(int[] pokemonIds)
    {
        int pokemon1id = pokemonIds[0];
        int pokemon2id = pokemonIds[1];
        var pokeFuseResponse = new PokeFuseResponse(pokemon1id, pokemon2id);

        var url1 = $"https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers/{pokemon1id}.{pokemon2id}.png";
        var url2 = $"https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers/{pokemon2id}.{pokemon1id}.png";

        var url1_404 = await CheckFor404(url1);
        var url2_404 = await CheckFor404(url2);

        if (url1_404 && url2_404) return null;

        if(!url1_404) {
            pokeFuseResponse.ImageUrl1 = url1;
        }
        if(!url2_404) {
            pokeFuseResponse.ImageUrl2 = url2;
        }
        
        return pokeFuseResponse;
    }

    static async Task<bool> CheckFor404(string url)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return response.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
    }
}