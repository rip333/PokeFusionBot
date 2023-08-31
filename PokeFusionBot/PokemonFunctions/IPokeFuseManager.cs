namespace PokemonFunctions;
public interface IPokeFuseManager
{
    PokeFuseResponse? GetFuseFromMessage(string text);
    PokeFuseResponse GetPokeFuseFromPokemonIds(int[] pokemonIds);
}
