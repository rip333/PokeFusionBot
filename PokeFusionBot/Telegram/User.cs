namespace Telegram;

public class User
{

    public long Id { get; set; }

    public bool IsBot { get; set; }

    public string FirstName { get; set; } = default!;

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? LanguageCode { get; set; }

    public bool? IsPremium { get; set; }

    public bool? AddedToAttachmentMenu { get; set; }

    public bool? CanJoinGroups { get; set; }

    public bool? CanReadAllGroupMessages { get; set; }

    public bool? SupportsInlineQueries { get; set; }

    public override string ToString() =>
        $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")} ({Id})";
}