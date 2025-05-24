using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PixelLib.Models;
using PixelLib.Utils;

namespace PixelLib.Http
{
    public class KersysHttp(HttpClient httpClient)
    {
        private const string CREATE_NEW_TOKEN_PATH = "";
        private const string GET_USER_DATA_PATH = "portabilidade/getuser";
        private const string AUTHORIZE_DATA_PORTABILITY = "portabilidade/authorize";
        private const string AUTH = "portabilidade/auth";

        private readonly HttpClient _httpClient = httpClient;

        public async Task Authenticate(CancellationToken token)
        {
            var request = new
            {
                clientId = "b428ed96-9499-468b-a988-490d723c66bc",
                clientSecret = "37a67d50-6b37-487b-b8b0-6abb0c1b0d64"
            };

            using StringContent content = new(JsonUtils.Serialize(request), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(AUTH, content, token);
            if(!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Acesso nao autorizado");
            }

            Stream stream = await response.Content.ReadAsStreamAsync(token);
            var auth = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(utf8Json: stream, cancellationToken: token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth["token"]);
        }

        public async Task<KersysAuthToken?> CreateNewAccessToken(KersysKeys libKeys, CancellationToken token)
        {
            var request = new
            {
                chave = libKeys.PublicKey
            };

            using StringContent content = new(JsonUtils.Serialize(request), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(CREATE_NEW_TOKEN_PATH, content, token);
            _ = response.EnsureSuccessStatusCode();

            Stream stream = await response.Content.ReadAsStreamAsync(token);
            KersysAuthToken? data = await JsonSerializer.DeserializeAsync<KersysAuthToken>(utf8Json: stream, cancellationToken: token);
            return data;
        }

        public async Task<Response?> GetUserData(string clientId, KersysParameters parameters, string aesKey, string aesIv, byte[] body, CancellationToken token)
        {
            var request = new
            {
                tempoExpiracao = parameters.ExpirationDate.ToString("yyyy-MM-ddThh:mm:ss"),
                clientID = Convert.ToInt32(clientId),
                usuarioID = Convert.ToBase64String(body),
                aesKey = aesKey,
                aesIv = aesIv
            };

            using StringContent content = new(JsonUtils.Serialize(request), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{GET_USER_DATA_PATH}", content, token);
            _ = response.EnsureSuccessStatusCode();

            Stream stream = await response.Content.ReadAsStreamAsync(token);
            Response? data = await JsonSerializer.DeserializeAsync<Response>(utf8Json: stream, cancellationToken: token);
            return data;
        }

        public async Task Authorize(bool accepted, string userHash, CancellationToken token)
        {
            var request = new
            {
                aceito = accepted,
                hashConfirmacao = userHash,
            };

            using StringContent content = new(JsonUtils.Serialize(request), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{AUTHORIZE_DATA_PORTABILITY}", content, token);
            _ = response.EnsureSuccessStatusCode();
        }
    }
}
