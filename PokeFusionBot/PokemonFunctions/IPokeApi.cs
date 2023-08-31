using PokeApiNet;

namespace PokemonFunctions;

public interface IPokeApi {
    Task<Pokemon> GetPokemonById(int id);
}