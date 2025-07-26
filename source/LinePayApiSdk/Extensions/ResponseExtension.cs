using LinePayApiSdk.DTOs;

namespace LinePayApiSdk.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task<TResponse> ToResult<TResponse>(this Task<HttpResponseMessage> responseTask)
            where TResponse : LinePayApiResponseBase
        {
            var response = await responseTask;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            var isJsonFormat = (await response.Content.ReadAsStringAsync())
                .TryParseJson<TResponse>(out var jsonResult, out var errMsg);

            return isJsonFormat
                ? jsonResult
                : throw new InvalidOperationException(errMsg);
        }
    }
}