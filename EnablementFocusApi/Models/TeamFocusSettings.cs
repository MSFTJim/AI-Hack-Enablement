namespace EnablementFocusApi.Models;

public sealed class TeamFocusSettings
{
    public const string SectionName = "TeamFocusSettings";

    public Dictionary<string, TeamMember> TeamMembers { get; } = new(StringComparer.OrdinalIgnoreCase);
}
