using PixelLib.Models;

namespace PixelLib
{
    public interface IKersysApi
    {
        Task CreateNewAccessToken(CancellationToken cancellationToken = default);
        Task<Data> GetUserDataAsync(string userId, KersysParameters parameters, CancellationToken cancellationToken = default);
        Task Authorize(bool accepted, string userHash, CancellationToken cancellationToken = default);
    }
}
