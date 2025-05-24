using PixelLib.Models;

namespace PixelLib
{
    public class KersysApi(KersysService service) : IKersysApi
    {
        private readonly KersysService _service = service;

        public async Task CreateNewAccessToken(CancellationToken cancellationToken = default)
        {
            await _service.CreateNewAccessToken(cancellationToken);
        }

        public async Task<Data> GetUserDataAsync(string userId, KersysParameters parameters, CancellationToken cancellationToken = default)
        {
            return await _service.GetUserData(userId, parameters, cancellationToken);
        }

        public Data DecryptUser(string user, string iv, string aes)
        {
            return _service.DecryptUser(user, iv, aes);
        }
    }
}
