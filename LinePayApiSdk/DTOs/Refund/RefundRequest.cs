using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Refund
{
    public class RefundRequest
    {
        /// <summary>
        /// 退款金額。返回空值的話，進行全部退款。
        /// </summary>
        [JsonPropertyName("refundAmount")]
        public decimal? RefundAmount { get; set; }

        /// <summary>
        /// 額外選項。
        /// </summary>
        [JsonPropertyName("options")]
        public RefundOptions Options { get; set; }
    }

    /// <summary>
    /// 表示退款請求的額外選項。
    /// </summary>
    public class RefundOptions
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