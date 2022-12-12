
namespace CG.Purple.Clients;

/// <summary>
/// This interface represents a factory for creating <see cref="IPurpleHttpClient"/>
/// instances.
/// </summary>
public interface IPurpleHttpClientFactory
{
    /// <summary>
    /// This method creates and returns a <see cref="IPurpleHttpClient"/>
    /// instance.
    /// </summary>
    /// <returns>An <see cref="IPurpleHttpClient"/> instance.</returns>
    IPurpleHttpClient CreateClient();
}
