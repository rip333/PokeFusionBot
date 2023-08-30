namespace Pokemon;

public class PokeFuseResponse
{
    public PokeFuseResponse(int id1, int id2){
        Pokemon1 = new Pokemon(id1);
        Pokemon2 = new Pokemon(id2);
    }

    public string? ImageUrl1 { get; set; }
    public string? ImageUrl2 { get; set; }
    public Pokemon Pokemon1 { get; set; }
    public Pokemon Pokemon2 { get; set; }
    public string FusedName() {
        return Utilities.CombineWords(Pokemon1.Name, Pokemon2.Name);
    }

    public string GetCaption1() {
        return $"{Utilities.UppercaseFirstLetter(Pokemon1.Name)} + {Utilities.UppercaseFirstLetter(Pokemon2.Name)} = {FusedName()}";
    }

        public string GetCaption2() {
        return $"{Utilities.UppercaseFirstLetter(Pokemon2.Name)} + {Utilities.UppercaseFirstLetter(Pokemon1.Name)} = {FusedName()}";
    }
}

public class Pokemon {
    public Pokemon(int id) {
        Id = id;
        Name = PokeData.dict.FirstOrDefault(x => x.Value == id).Key;
    }

    public string Name { get; set; }
    public int Id { get; set; }
    public string? Type { get; set; }
}