using PixelLib.Models;

namespace PixelLib
{
    public interface IKersysService
    {
        Task CreateNewAccessToken(CancellationToken token);
        Task<Data> GetUserData(string request, KersysParameters parameters, CancellationToken token);
        Task Authorize(bool accepted, string userHash, CancellationToken cancellationToken = default);
    }
}
