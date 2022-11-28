namespace WorkerQueuePoC.Workers.Models;

using System.Threading.Channels;
using MediatR;

internal class DistinctRequestChannel<TRequest>
    where TRequest : IRequest

{
    private readonly Channel<TRequest> channel;
    private readonly HashSet<TRequest> requests;

    public DistinctRequestChannel(BoundedChannelOptions options)
    {
        this.channel = Channel.CreateBounded<TRequest>(options);
        this.requests = new HashSet<TRequest>();
    }

    public async Task<TRequest> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var request = await channel.Reader.ReadAsync(cancellationToken);

        this.requests.Remove(request);

        return request;
    }

    public async Task EnqueueAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (!this.requests.Contains(request))
        {
            await this.channel.Writer.WriteAsync(request, cancellationToken);
            this.requests.Add(request);
        }
    }
}
