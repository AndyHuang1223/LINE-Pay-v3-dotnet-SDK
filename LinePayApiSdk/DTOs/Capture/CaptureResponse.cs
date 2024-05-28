using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Capture
{
    public class CaptureResponse : LinePayApiResponseBase
    {
        /// <summary>
        /// 交易資訊。
        /// </summary>
        [JsonPropertyName("info")]
        public List<TransactionInfo> Info { get; set; }
    }

    /// <summary>
    /// 表示交易資訊。
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        /// 在請求付款時，商家回應的訂單編號。
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 作為請求付款的結果，回應的交易序號（19個字符）。
        /// </summary>
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// 付款資訊列表。
        /// </summary>
        [JsonPropertyName("payInfo")]
        public List<PayInfo> PayInfo { get; set; }
    }

    /// <summary>
    /// 表示付款資訊。
    /// </summary>
    public class PayInfo
    {
        /// <summary>
        /// 付款方式。信用卡：CREDIT_CARD，餘額：BALANCE，折扣：DISCOUNT (發票金額須扣除)，LINE POINTS：POINT (預設不顯示)。
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; }

        /// <summary>
        /// 付款金額。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}