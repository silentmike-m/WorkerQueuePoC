namespace WorkerQueuePoC.Users.Models;

public sealed record UserToCreate
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
}
