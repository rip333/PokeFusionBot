using PokemonFunctions;

namespace PokeFusionBot.ChatGptFunctions;

public static class Prompts
{
    public static string FuseBattle(PokeFuseResponse pokeFuseResponse1, PokeFuseResponse pokeFuseResponse2)
    {
       return @"Pokemon has a new concept called fusions, where two pokemon are fused into one. "
       + "This fusions stats are somewhere between the two, depending on who is the head and who is the body. "
       + $"Generate the Gameboy console text of a pokemon fight between {pokeFuseResponse1.FusedName(1)} ({pokeFuseResponse1.Pokemon1.Name} & {pokeFuseResponse1.Pokemon2.Name}) and {pokeFuseResponse2.FusedName(1)} ({pokeFuseResponse2.Pokemon1.Name} & {pokeFuseResponse2.Pokemon2.Name}). "
       + "Focus on the statistics of the fight, including health, critical hits, status effects, items, etc. "
       + "Do not give any additional explanation besides the console text.";
    }

}