using System.Text.RegularExpressions;
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
        if (!message.Contains("battle")) return null;
        ExtractIds(message, out int[] firstIds, out int[] secondIds);
        var fuse1 = new PokeFuseResponse(firstIds);
        var fuse2 = new PokeFuseResponse(secondIds);
        var chatGptResponse = await _chatGpt.Ask(Prompts.FuseBattle(fuse1, fuse2));
        var header = $"{fuse1.FusedName(1)} ({Utilities.UppercaseFirstLetter(fuse1.Pokemon1.Name)} & {Utilities.UppercaseFirstLetter(fuse1.Pokemon2.Name)})\n "
            + $"VERSUS\n {fuse2.FusedName(1)} ({Utilities.UppercaseFirstLetter(fuse2.Pokemon1.Name)} & {Utilities.UppercaseFirstLetter(fuse2.Pokemon2.Name)})\n";
        return $"```{header}\n{RemoveNoteFromString(chatGptResponse)}\n```";
    }

    public static void ExtractIds(string text, out int[] ids1, out int[] ids2)
    {
        var match = Regex.Match(text, @"(\d+)\.(\d+)\s+(\d+)\.(\d+)");
        if (match.Success)
        {
            ids1 = new int[] { int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value) };
            ids2 = new int[] { int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value) };
        }
        else
        {
            ids1 = ids2 = new int[0];
        }
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

    public static string RemoveNoteFromString(string content)
    {
        string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        int noteLineIndex = -1;

        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (lines[i].StartsWith("(Note:"))
            {
                noteLineIndex = i;
                break;
            }
        }

        if (noteLineIndex != -1)
        {
            string[] newLines = new string[noteLineIndex];
            Array.Copy(lines, newLines, noteLineIndex);
            return string.Join(Environment.NewLine, newLines);
        }

        return content;
    }

}