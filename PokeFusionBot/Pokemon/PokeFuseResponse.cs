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

    public string ImageUrl1 { get; set; }
    public string ImageUrl2 { get; set; }
    public Pokemon Pokemon1 { get; set; }
    public Pokemon Pokemon2 { get; set; }
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

    public string GetWebpUrl1() {
        return $"fuse.webp";
    }

    public string GetWebpUrl2() {
        return $"fuse.webp";
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