namespace WorkerQueuePoC.Users.Commands;

using MediatR;
using WorkerQueuePoC.Users.Models;

public sealed record CreateUser : IRequest
{
    public UserToCreate User { get; init; } = new();
}
