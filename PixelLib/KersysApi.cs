using PixelLib.Models;

namespace PixelLib
{
    public class KersysApi(IKersysService service) : IKersysApi
    {
        private readonly IKersysService _service = service;

        public async Task CreateNewAccessToken(CancellationToken cancellationToken = default)
        {
            await _service.CreateNewAccessToken(cancellationToken);
        }

        public async Task<Data> GetUserDataAsync(string userId, KersysParameters parameters, CancellationToken cancellationToken = default)
        {
            return await _service.GetUserData(userId, parameters, cancellationToken);
        }

        public async Task Authorize(bool accepted, string userHash, CancellationToken cancellationToken = default)
        {
            await _service.Authorize(accepted, userHash, cancellationToken);
        }
    }
}
