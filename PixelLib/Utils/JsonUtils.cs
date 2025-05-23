using System.Text.Encodings.Web;
using System.Text.Json;

namespace PixelLib.Utils
{
    public static class JsonUtils
    {
        public static string Serialize(object request)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(request, options);
        }
    }
}

