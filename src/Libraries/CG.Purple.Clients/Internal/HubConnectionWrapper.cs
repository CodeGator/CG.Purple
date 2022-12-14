
namespace CG.Purple.Clients.Internal;

/// <summary>
/// This class is a default implementation of the <see cref="IHubConnection"/>
/// interface.
/// </summary>
internal class HubConnectionWrapper : IHubConnection
{
    /// <inheritdoc/>
    internal protected readonly HubConnection _innerHubConnection;

    /// <inheritdoc/>
    public string? ConnectionId => _innerHubConnection.ConnectionId;

    /// <inheritdoc/>
    public TimeSpan HandshakeTimeout
    {
        get { return _innerHubConnection.HandshakeTimeout; }
        set { _innerHubConnection.HandshakeTimeout = value; }
    }

    /// <inheritdoc/>
    public TimeSpan KeepAliveInterval
    {
        get { return _innerHubConnection.KeepAliveInterval; }
        set { _innerHubConnection.KeepAliveInterval = value; }
    }

    /// <inheritdoc/>
    public TimeSpan ServerTimeout
    {
        get { return _innerHubConnection.ServerTimeout; }
        set { _innerHubConnection.ServerTimeout = value; }
    }

    /// <inheritdoc/>
    public HubConnectionState State
    {
        get { return _innerHubConnection.State;  }
    }

    /// <inheritdoc/>
    public event Func<Exception?, Task>? Closed;

    /// <inheritdoc/>
    public event Func<string?, Task>? Reconnected;

    /// <inheritdoc/>
    public event Func<Exception?, Task>? Reconnecting;

    /// <summary>
    /// This constructor creates a new instance of the <see cref="HubConnectionWrapper"/>
    /// class.
    /// </summary>
    /// <param name="hubConnection">The SignalR hub to use for this wrapper.</param>
    public HubConnectionWrapper(
        HubConnection hubConnection
        )
    {
        _innerHubConnection = hubConnection;

        _innerHubConnection.Closed += _innerHubConnection_Closed;
        _innerHubConnection.Reconnected += _innerHubConnection_Reconnected;
        _innerHubConnection.Reconnecting += _innerHubConnection_Reconnecting;
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return _innerHubConnection.DisposeAsync();
    }

    /// <inheritdoc/>
    public Task<object?> InvokeCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.InvokeCoreAsync(methodName, returnType, args, cancellationToken);
    }

    /// <inheritdoc/>
    public IDisposable On(string methodName, Type[] parameterTypes, Func<object?[], object, Task> handler, object state)
    {
        return _innerHubConnection.On(methodName, parameterTypes, handler, state);
    }

    /// <inheritdoc/>
    public void Remove(string methodName)
    {
        _innerHubConnection.Remove(methodName);
    }

    /// <inheritdoc/>
    public Task SendCoreAsync(string methodName, object?[] args, CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.SendCoreAsync(methodName, args, cancellationToken);  
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.StartAsync(cancellationToken);   
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.StopAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<ChannelReader<object?>> StreamAsChannelCoreAsync(string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.StreamAsChannelCoreAsync(methodName.ToLower(), returnType, args, cancellationToken);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<TResult> StreamAsyncCore<TResult>(string methodName, object?[] args, CancellationToken cancellationToken = default)
    {
        return _innerHubConnection.StreamAsyncCore<TResult>(methodName, args, cancellationToken);
    }

    /// <inheritdoc/>
    private Task _innerHubConnection_Reconnecting(Exception? arg)
    {
        return Reconnecting?.Invoke(arg) ?? Task.CompletedTask;
    }

    /// <inheritdoc/>
    private Task _innerHubConnection_Reconnected(string? arg)
    {
        return Reconnected?.Invoke(arg) ?? Task.CompletedTask;
    }

    /// <inheritdoc/>
    private Task _innerHubConnection_Closed(Exception? arg)
    {
        return Closed?.Invoke(arg) ?? Task.CompletedTask;
    }
}
