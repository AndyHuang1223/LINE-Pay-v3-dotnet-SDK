using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Confirm
{
    /// <summary>
    /// 表示確認付款請求的主體。
    /// </summary>
    public class ConfirmRequest
    {
        /// <summary>
        /// 付款金額。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 貨幣(ISO 4217)。支援下列貨幣：USD, JPY, TWD, THB。
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}