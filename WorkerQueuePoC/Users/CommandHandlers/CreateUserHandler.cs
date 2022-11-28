namespace WorkerQueuePoC.Users.CommandHandlers;

using MediatR;
using WorkerQueuePoC.Users.Commands;

internal sealed class CreateUserHandler : IRequestHandler<CreateUser>
{
    private readonly ILogger<CreateUserHandler> logger;

    public CreateUserHandler(ILogger<CreateUserHandler> logger)
        => this.logger = logger;

    public async Task<Unit> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to create user with id '{UserId}' and name '{UserName}'", request.User.Id, request.User.Name);

        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
