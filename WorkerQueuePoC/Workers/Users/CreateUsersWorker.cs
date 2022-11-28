namespace WorkerQueuePoC.Workers.Users;

using WorkerQueuePoC.Users.Commands;
using WorkerQueuePoC.Users.Models;

internal sealed class CreateUsersWorker : BackgroundService
{
    private const int DELAY_TIMEOUT_IN_MINUTES = 2;

    private readonly ILogger<CreateUsersWorker> logger;
    private readonly CreateUsersQueue queue;

    public CreateUsersWorker(ILogger<CreateUsersWorker> logger, CreateUsersQueue queue)
        => (this.logger, this.queue) = (logger, queue);

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Worker {Worker} is stopping.", nameof(CreateUsersWorker));

        await base.StopAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var requestOne = GetCreateUserRequest(new Guid("D664DACF-5D0B-4905-92EB-8B590E9D9AFC"), "Test 01");
                var requestTWo = GetCreateUserRequest(new Guid("05DC6C60-AAEA-432D-9A88-A74CC0E64810"), "Test 02");
                var requestThree = GetCreateUserRequest(new Guid("7AB96783-2807-4A22-8F66-18DD16A419F3"), "Test 03");
                var requestFour = GetCreateUserRequest(new Guid("C1F3E363-F026-4646-8DC9-91D9FF657624"), "Test 04");

                await this.queue.QueueRequestAsync(requestOne, cancellationToken);
                await this.queue.QueueRequestAsync(requestTWo, cancellationToken);
                await this.queue.QueueRequestAsync(requestThree, cancellationToken);
                await this.queue.QueueRequestAsync(requestThree, cancellationToken);
                await this.queue.QueueRequestAsync(requestThree, cancellationToken);
                await this.queue.QueueRequestAsync(requestFour, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred executing task work item.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(DELAY_TIMEOUT_IN_MINUTES), cancellationToken);
            }
        }
    }

    private static CreateUser GetCreateUserRequest(Guid userId, string userName)
    {
        var user = new UserToCreate
        {
            Id = userId,
            Name = userName,
        };

        var request = new CreateUser
        {
            User = user,
        };

        return request;
    }
}
