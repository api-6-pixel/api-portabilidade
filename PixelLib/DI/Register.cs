using Microsoft.Extensions.DependencyInjection;
using PixelLib.Http;

namespace PixelLib.DI
{
    public static class Register
    {
        public static void RegisterKersysApi(this IServiceCollection services, Action<HttpClient> configureClient)
        {
            _ = services.AddSingleton<IKersysApi, KersysApi>();
            _ = services.AddSingleton<KersysService>();
            _ = services.AddHttpClient<KersysHttp>(configureClient);
        }
    }
}
