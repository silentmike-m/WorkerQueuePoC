namespace WorkerQueuePoC.Workers.Users;

using System.Threading.Channels;
using MediatR;
using WorkerQueuePoC.Users.Commands;
using WorkerQueuePoC.Workers.Models;

internal sealed class CreateUsersQueue
{
    private const int QUEUE_CAPACITY = 5;

    private readonly IMediator mediator;
    private readonly DistinctRequestChannel<CreateUser> queue;
    private readonly SemaphoreSlim signal;

    public CreateUsersQueue(IMediator mediator)
    {
        var options = new BoundedChannelOptions(QUEUE_CAPACITY)
        {
            FullMode = BoundedChannelFullMode.Wait,
        };

        this.mediator = mediator;
        this.queue = new DistinctRequestChannel<CreateUser>(options);
        this.signal = new SemaphoreSlim(1, 1);

        this.requestQueued += OnRequestQueued;
    }

    public async Task QueueRequestAsync(CreateUser request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        await this.queue.EnqueueAsync(request, cancellationToken);

        this.requestQueued.Invoke();
    }

    private async void OnRequestQueued()
    {
        await this.signal.WaitAsync(CancellationToken.None);

        var request = await this.queue.DequeueAsync(CancellationToken.None);

        _ = await this.mediator.Send(request, CancellationToken.None);

        this.signal.Release();
    }

    private event Action requestQueued;
}
