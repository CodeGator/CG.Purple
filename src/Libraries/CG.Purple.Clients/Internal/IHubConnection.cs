
namespace CG.Purple.Clients.Internal;

/// <summary>
/// This interface represents a SignalR hub.
/// </summary>
/// <remarks>
/// <para>
/// The purpose of this interface is for testability. In short, the Microsoft
/// SignalR HubConnection type is simply not mockable, at all. Or at least, it
/// isn't mockable in any way that I understand. If you know a way to mock the
/// HubConnection type, please let me know.
/// </para>
/// <para>
/// This type wraps a standard SignalR HubConnection instance and defers to
/// that object at runtime. During testing, we can then mock up the IHubConnection
/// type, and use that, instead.
/// </para>
/// </remarks>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IHubConnection
{
    event Func<Exception?, Task>? Closed;
    event Func<string?, Task>? Reconnected;
    event Func<Exception?, Task>? Reconnecting;

    string? ConnectionId { get; }
    TimeSpan HandshakeTimeout { get; set; }
    TimeSpan KeepAliveInterval { get; set; }
    TimeSpan ServerTimeout { get; set; }
    HubConnectionState State { get; }

    ValueTask DisposeAsync();

    Task<object?> InvokeCoreAsync(
        string methodName,
        Type returnType, 
        object?[] args, 
        CancellationToken cancellationToken = default
        );

    IDisposable On(
        string methodName, 
        Type[] parameterTypes, 
        Func<object?[], object, Task> handler,
        object state
        );

    void Remove(string methodName);

    Task SendCoreAsync(
        string methodName, 
        object?[] args, 
        CancellationToken cancellationToken = default
        );

    Task StartAsync(
        CancellationToken cancellationToken = default
        );

    Task StopAsync(
        CancellationToken cancellationToken = default
        );

    Task<ChannelReader<object?>> StreamAsChannelCoreAsync(
        string methodName, 
        Type returnType, 
        object?[] args, 
        CancellationToken cancellationToken = default
        );

    IAsyncEnumerable<TResult> StreamAsyncCore<TResult>(
        string methodName, 
        object?[] args, 
        CancellationToken cancellationToken = default
        );
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member