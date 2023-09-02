using PokeFusionBot.ChatGptFunctions;

namespace PokemonFunctions;

public class BattleManager : IBattleManager
{
    public readonly IChatGptWrapper _chatGpt;

    public BattleManager(IChatGptWrapper chatGptWrapper)
    {
        _chatGpt = chatGptWrapper;
    }

    public async Task<string?> HandleBattle(string message)
    {
        if (ExtractIds(message, out int[] firstIds, out int[] secondIds))
        {
            var fuse1 = new PokeFuseResponse(firstIds);
            var fuse2 = new PokeFuseResponse(secondIds);
            var chatGptResponse = await _chatGpt.Ask(Prompts.FuseBattle(fuse1, fuse2));
            return "```\n" + chatGptResponse + "\n```";
        }
        return null;
    }

    private static bool ExtractIds(string input, out int[] firstIds, out int[] secondIds)
    {
        firstIds = new int[0];
        secondIds = new int[0];

        // Split the input string into words
        string[] words = input.Split(' ');

        // Check if the input contains at least four words
        if (words.Length < 3 || words[0] != "battle")
        {
            return false;
        }

        // Initialize arrays to store extracted IDs
        firstIds = new int[words.Length - 1];
        secondIds = new int[words.Length - 1];

        // Extract and parse the IDs
        for (int i = 1; i < words.Length; i++)
        {
            string[] idParts = words[i].Split('.');
            if (idParts.Length != 2 || !int.TryParse(idParts[0], out firstIds[i - 1]) || !int.TryParse(idParts[1], out secondIds[i - 1]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CheckStringFormat(string input)
    {
        // Split the input string into words
        string[] words = input.Split(' ');

        // Check if the input contains at least four words
        if (words.Length < 3 || words[0] != "battle")
        {
            return false;
        }

        // Check if the remaining words have the required format
        for (int i = 1; i < words.Length; i++)
        {
            string[] idParts = words[i].Split('.');
            if (idParts.Length != 2 || !int.TryParse(idParts[0], out _) || !int.TryParse(idParts[1], out _))
            {
                return false;
            }
        }

        return true;
    }

}