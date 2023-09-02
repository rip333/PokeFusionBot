using Images;
using PokeApiNet;

namespace PokemonFunctions;

public class PokeFuseResponse
{
    public PokeFuseResponse(int id1, int id2, string imageUrl1, string imageUrl2)
    {
        Pokemon1 = PokemonFromId(id1);
        Pokemon2 = PokemonFromId(id2);
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
        Pokemon1 = PokemonFromId(pokemonIds[0]);
        Pokemon2 = PokemonFromId(pokemonIds[1]);
        ImageUrl1 = $"{Constants.SpriteBaseUrl}/{pokemon1id}.{pokemon2id}.png";
        ImageUrl2 = $"{Constants.SpriteBaseUrl}/{pokemon2id}.{pokemon1id}.png";
    }

    private static Pokemon PokemonFromId(int id)
    {
        return new Pokemon()
        {
            Id = id,
            Name = PokeData.GetNameFromId(id) ?? ""
        };
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
        return $"{Utilities.UppercaseFirstLetter(Pokemon1.Name)} + {Utilities.UppercaseFirstLetter(Pokemon2.Name)} = {FusedName(1)}. (ID: {Pokemon1.Id}.{Pokemon2.Id})";
    }

    public string GetCaption2()
    {
        return $"{Utilities.UppercaseFirstLetter(Pokemon2.Name)} + {Utilities.UppercaseFirstLetter(Pokemon1.Name)} = {FusedName(2)} (ID: {Pokemon2.Id}.{Pokemon1.Id})";
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

    public async Task<PokeFuseResponse> PopulatePokemonData(IPokeApi pokeApi)
    {
        PokeFuseResponse populatedFuseResponse = this;
        populatedFuseResponse.Pokemon1 = await pokeApi.GetPokemonById(Pokemon1.Id);
        populatedFuseResponse.Pokemon2 = await pokeApi.GetPokemonById(Pokemon2.Id);
        return populatedFuseResponse;
    }
}