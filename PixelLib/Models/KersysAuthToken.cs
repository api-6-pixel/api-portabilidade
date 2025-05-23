
using System.Text.Json.Serialization;

namespace PixelLib.Models
{
    public class KersysAuthToken
    {
        [JsonPropertyName("id")]
        public required string ClientId { get; set; }

        [JsonPropertyName("minhaChavePub")]
        public required string PublicKey { get; set; }
    }
}
