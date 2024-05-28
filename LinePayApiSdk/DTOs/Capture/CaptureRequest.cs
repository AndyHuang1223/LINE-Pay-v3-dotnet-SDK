using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Capture
{
    public class CaptureRequest
    {
        /// <summary>
        /// 付款金額。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 貨幣（ISO 4217）。支援下列貨幣: USD, JPY, TWD, THB。
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 額外選項。
        /// </summary>
        [JsonPropertyName("options")]
        public CaptureOptions Options { get; set; }
    }

    /// <summary>
    /// 表示捕捉付款的額外選項。
    /// </summary>
    public class CaptureOptions
    {
        /// <summary>
        /// 額外選項中的點數限制資訊。
        /// </summary>
        [JsonPropertyName("extra")]
        public Extra Extra { get; set; }
    }

    /// <summary>
    /// 表示點數限制資訊。
    /// </summary>
    public class Extra
    {
        /// <summary>
        /// 點數限制資訊。
        /// </summary>
        [JsonPropertyName("promotionRestriction")]
        public PromotionRestriction PromotionRestriction { get; set; }
    }

    /// <summary>
    /// 表示點數限制資訊的具體內容。
    /// </summary>
    public class PromotionRestriction
    {
        /// <summary>
        /// 不可使用點數折抵的金額。
        /// </summary>
        [JsonPropertyName("useLimit")]
        public decimal UseLimit { get; set; }

        /// <summary>
        /// 不可回饋點數的金額。
        /// </summary>
        [JsonPropertyName("rewardLimit")]
        public decimal RewardLimit { get; set; }
    }
}