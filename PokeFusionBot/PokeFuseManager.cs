using FuzzySearch;

public class PokeFuseManager
{
    public async static Task<string> GetFuseFromMessage(string text)
    {
        var splitString = text.Split(" ");
        if (splitString.Length > 3) return "";

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
            var url = await GetUrlsFromPokemonIds(matches);
            return url;
        }
        return "";
    }

    public static async Task<string> GetUrlsFromPokemonIds(int[] pokemonIds)
    {
        int pokemon1id = pokemonIds[0];
        int pokemon2id = pokemonIds[1];

        var url1 = $"https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers/{pokemon1id}.{pokemon2id}.png";
        var url2 = $"https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers/{pokemon2id}.{pokemon1id}.png";

        var url1_404 = await CheckFor404(url1);
        var url2_404 = await CheckFor404(url2);

        string url = "";
        if (url1_404 && url2_404) return url;
        if (!url1_404) url = url1;
        else url = url2;

        Console.WriteLine(url);
        return url;
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