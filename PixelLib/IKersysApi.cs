using PixelLib.Models;

namespace PixelLib
{
    public interface IKersysApi
    {
        Data DecryptUser(string user, string iv, string aes);
        Task CreateNewAccessToken(CancellationToken cancellationToken = default);
        Task<Data> GetUserDataAsync(string userId, KersysParameters parameters, CancellationToken cancellationToken = default);
    }
}
