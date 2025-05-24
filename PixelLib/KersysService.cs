using PixelLib.Encription;
using PixelLib.Http;
using PixelLib.Models;

namespace PixelLib
{
    public class KersysService(KersysHttp kersysHttp) : IKersysService
    {
        //Chave publica e privada da lib
        private KersysKeys? libKeys;

        //Chave da api
        private KersysAuthToken? apiAccessToken;

        public async Task CreateNewAccessToken(CancellationToken token)
        {
            await kersysHttp.Authenticate(token);

            libKeys = AsymmetricManager.GenerateKeys();
            apiAccessToken = await kersysHttp.CreateNewAccessToken(libKeys, token);
            ArgumentNullException.ThrowIfNull(apiAccessToken);
        }

        public async Task<Data> GetUserData(string request, KersysParameters parameters, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(apiAccessToken);
            ArgumentNullException.ThrowIfNull(libKeys);

            var symmetricKey = SymmetricManager.GenerateKeyAndIV();
            byte[] body = SymmetricManager.Encrypt(request, symmetricKey.Key, symmetricKey.IV);
            string aesKey = AsymmetricManager.Encrypt(symmetricKey.Key, apiAccessToken.PublicKey);
            string aesIV = AsymmetricManager.Encrypt(symmetricKey.IV, apiAccessToken.PublicKey);

            Response? response = await kersysHttp.GetUserData(apiAccessToken.ClientId, parameters, aesKey, aesIV, body, token);

            ArgumentNullException.ThrowIfNull(response);

            if (response.aesKey is null || response.aesIv is null)
            {
                return new Data(response.dados);
            }

            string newAesKey = AsymmetricManager.Decrypt(Convert.FromBase64String(response.aesKey), libKeys.PrivateKey);
            string newAesIv = AsymmetricManager.Decrypt(Convert.FromBase64String(response.aesIv), libKeys.PrivateKey);
            string result = SymmetricManager.Decrypt(Convert.FromBase64String(response.dados), symmetricKey.Key, symmetricKey.IV);
            return new Data(result);
        }

        public Data DecryptUser(string request, string iv, string aes)
        {
            string newAesKey = AsymmetricManager.Decrypt(Convert.FromBase64String(aes), libKeys.PrivateKey);
            string newAesIv = AsymmetricManager.Decrypt(Convert.FromBase64String(iv), libKeys.PrivateKey);
            string result = SymmetricManager.Decrypt(Convert.FromBase64String(request), Convert.FromBase64String(newAesKey), Convert.FromBase64String(newAesIv));
            return new Data(result);
        }


        public async Task Authorize(bool accepted, string userHash, CancellationToken cancellationToken = default)
        {
            await kersysHttp.Authorize(accepted, userHash, cancellationToken);
        }

    }
}
