namespace PokemonFunctions;

public interface IBattleManager {
    Task<string?> HandleBattle(string message);
}