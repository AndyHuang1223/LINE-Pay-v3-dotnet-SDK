using LinePayApiSdk.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LinePayApiSdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string SandboxApiUrl = "https://sandbox-api-pay.line.me";
        private const string ProductionApiUrl = "https://api-pay.line.me";
        private const int DefaultTimeoutSeconds = 60;

        /// <summary>
        /// 添加 LinePay API 服務到 DI 容器
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <param name="configureOptions">配置選項的委託</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddLinePay(this IServiceCollection services, Action<LinePayApiOptions> configureOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            // 註冊選項
            services.Configure(configureOptions);

            // 註冊服務
            RegisterLinePayServices(services);

            return services;
        }

        /// <summary>
        /// 添加 LinePay API 服務到 DI 容器（使用配置節）
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <param name="configuration">配置</param>
        /// <param name="sectionName">配置節名稱</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddLinePay(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration, string sectionName = "LinePay")
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // 註冊選項
            services.Configure<LinePayApiOptions>(configuration.GetSection(sectionName));

            // 註冊服務
            RegisterLinePayServices(services);

            return services;
        }

        /// <summary>
        /// 註冊 LinePay 相關服務
        /// </summary>
        /// <param name="services">服務集合</param>
        private static void RegisterLinePayServices(IServiceCollection services)
        {
            // 註冊 HttpClient
            services.AddHttpClient<ILinePayApi, LinePayApi>(ConfigureHttpClient);

            // 註冊 LinePay API 服務
            services.AddScoped<ILinePayApi, LinePayApi>();
        }

        /// <summary>
        /// 配置 HttpClient
        /// </summary>
        /// <param name="serviceProvider">服務提供者</param>
        /// <param name="httpClient">HttpClient 實例</param>
        private static void ConfigureHttpClient(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            var options = serviceProvider.GetRequiredService<IOptions<LinePayApiOptions>>().Value;

            // 設定 BaseAddress
            httpClient.BaseAddress = GetBaseAddress(options);

            // 設定 Timeout
            httpClient.Timeout = TimeSpan.FromSeconds(DefaultTimeoutSeconds);
        }

        /// <summary>
        /// 根據選項取得 Base Address
        /// </summary>
        /// <param name="options">LinePay API 選項</param>
        /// <returns>Base Address Uri</returns>
        private static Uri GetBaseAddress(LinePayApiOptions options)
        {
            return options.IsSandBox 
                ? new Uri(SandboxApiUrl) 
                : new Uri(ProductionApiUrl);
        }
    }
}
