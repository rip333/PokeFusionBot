using Images;

namespace Pokemon;

public class PokeFuseResponse
{
    public PokeFuseResponse(int id1, int id2, string imageUrl1, string imageUrl2)
    {
        Pokemon1 = new Pokemon(id1);
        Pokemon2 = new Pokemon(id2);
        ImageUrl1 = imageUrl1;
        ImageUrl2 = imageUrl2;
    }

    public static PokeFuseResponse Random()
    {
        var pfr = new PokeFuseResponse(0, 0, "", "")
        {
            IsRandom = true,
        };
        return pfr;
    }

    public PokeFuseResponse(int[] pokemonIds)
    {
        int pokemon1id = pokemonIds[0];
        int pokemon2id = pokemonIds[1];
        Pokemon1 = new Pokemon(pokemonIds[0]);
        Pokemon2 = new Pokemon(pokemonIds[1]);
        var baseUrl = "https://raw.githubusercontent.com/Aegide/custom-fusion-sprites/main/CustomBattlers";
        ImageUrl1 = $"{baseUrl}/{pokemon1id}.{pokemon2id}.png";
        ImageUrl2 = $"{baseUrl}/{pokemon2id}.{pokemon1id}.png";
    }

    public string ImageUrl1 { get; set; }
    public string ImageUrl2 { get; set; }
    public Pokemon Pokemon1 { get; set; }
    public Pokemon Pokemon2 { get; set; }
    public bool IsRandom { get; set; } = false;
    public string FusedName(int i = 1)
    {
        if (Pokemon1.Name == Pokemon2.Name)
        {
            return Pokemon1.Name.ToUpper();
        }
        if (i == 1)
        {
            return Utilities.CombineWords(Pokemon1.Name, Pokemon2.Name);
        }
        else
        {
            return Utilities.CombineWords(Pokemon2.Name, Pokemon1.Name);
        }
    }

    public string GetCaption1()
    {
        return $"{Utilities.UppercaseFirstLetter(Pokemon1.Name)} + {Utilities.UppercaseFirstLetter(Pokemon2.Name)} = {FusedName(1)}";
    }

    public string GetCaption2()
    {
        return $"{Utilities.UppercaseFirstLetter(Pokemon2.Name)} + {Utilities.UppercaseFirstLetter(Pokemon1.Name)} = {FusedName(2)}";
    }

    public string GetWebpUrl()
    {
        return $"fuse.webp";
    }

    public async Task<PokeFuseResponse> GenerateRandomUntilValid(IImageManager imageManager)
    {
        var valid = false;
        PokeFuseResponse randomPokeFuseResponse = this;
        while (!valid)
        {
            var random = new int[]
            {
                PokeData.GetRandomValueFromDictionary(),
                PokeData.GetRandomValueFromDictionary()
            };
            randomPokeFuseResponse = new PokeFuseResponse(random);
            var image1Valid = !await imageManager.CheckFor404(randomPokeFuseResponse.ImageUrl1);
            var image2Valid = !await imageManager.CheckFor404(randomPokeFuseResponse.ImageUrl2);

            if (image1Valid || image2Valid) valid = true;
        }
        return randomPokeFuseResponse;
    }
}

public class Pokemon
{
    public Pokemon(int id)
    {
        Id = id;
        Name = PokeData.PokemonIdDictionary.FirstOrDefault(x => x.Value == id).Key;
    }

    public string Name { get; set; }
    public int Id { get; set; }
    public string? Type { get; set; }
}