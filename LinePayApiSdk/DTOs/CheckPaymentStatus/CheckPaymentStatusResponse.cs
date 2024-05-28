using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.CheckPaymentStatus
{
    public class CheckPaymentStatusResponse : LinePayApiResponseBase
    {
        /// <summary>
        /// 配送資訊。
        /// </summary>
        [JsonPropertyName("info")]
        public ShippingInfo Info { get; set; }
    }

    /// <summary>
    /// 表示配送資訊。
    /// </summary>
    public class ShippingInfo
    {
        /// <summary>
        /// 用戶所選的配送方式ID。
        /// </summary>
        [JsonPropertyName("methodId")]
        public string MethodId { get; set; }

        /// <summary>
        /// 運費。
        /// </summary>
        [JsonPropertyName("feeAmount")]
        public decimal FeeAmount { get; set; }
    }
}