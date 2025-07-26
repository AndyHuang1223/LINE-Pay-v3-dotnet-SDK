using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.PayPreapproved
{
    public class PayPreapprovedRequest
    {
        /// <summary>
        /// 商品名稱。
        /// </summary>
        [JsonPropertyName("productName")]
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// 付款金額。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 貨幣(ISO 4217)。支援如下貨幣：USD, JPY, TWD, THB。
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 商家唯一訂單編號。
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 購買是否完成。
        /// `true`: 授權/購買。
        /// `false`: 只完成授權，需要呼叫Capture API進行購買。
        /// </summary>
        [JsonPropertyName("capture")]
        public bool? Capture { get; set; }
    }
}