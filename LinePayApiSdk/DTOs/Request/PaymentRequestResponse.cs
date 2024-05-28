using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Request
{
    public class PaymentRequestResponse : LinePayApiResponseBase
    {
        /// <summary>
        /// 交易資訊。
        /// </summary>
        [JsonPropertyName("info")]
        public TransactionInfo Info { get; set; }
    }

    /// <summary>
    /// 表示交易資訊。
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        /// 交易序號。
        /// </summary>
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// 該代碼在LINE Pay可以代替掃描器使用。
        /// </summary>
        [JsonPropertyName("paymentAccessToken")]
        public string PaymentAccessToken { get; set; }

        /// <summary>
        /// 付款頁的URL。
        /// </summary>
        [JsonPropertyName("paymentUrl")]
        public PaymentUrl PaymentUrl { get; set; }
    }

    /// <summary>
    /// 表示付款頁的URL。
    /// </summary>
    public class PaymentUrl
    {
        /// <summary>
        /// 用來跳轉到付款頁的App URL。在應用程式發起付款請求時使用。在從商家應用跳轉到LINE Pay時使用。
        /// </summary>
        [JsonPropertyName("app")]
        public string App { get; set; }

        /// <summary>
        /// 用來跳轉到付款頁的Web URL。在網頁請求付款時使用。在跳轉到LINE Pay等待付款頁時使用。不經參數，直接跳轉到傳來的URL。在Desktop版，彈窗大小為Width：700px，Height：546px。
        /// </summary>
        [JsonPropertyName("web")]
        public string Web { get; set; }
    }
}