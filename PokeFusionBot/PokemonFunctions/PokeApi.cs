using PokeApiNet;

namespace PokemonFunctions;

public class PokeApi : IPokeApi {

    private PokeApiClient _pokeApiClient;
    public PokeApi(PokeApiClient pokeApiClient) {
        _pokeApiClient = pokeApiClient;
    }

    public async Task<Pokemon> GetPokemonById(int id) {
        return await _pokeApiClient.GetResourceAsync<Pokemon>(id);
    }
}