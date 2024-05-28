using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.PaymentDetail
{
    public class PaymentDetailRequest
    {
        /// <summary>
        /// 由LINE Pay建立的交易序號或退款序號。
        /// </summary>
        [JsonPropertyName("transactionId")]
        public List<long> TransactionId { get; set; }

        /// <summary>
        /// 商家訂單編號。
        /// </summary>
        [JsonPropertyName("orderId")]
        public List<string> OrderId { get; set; }

        /// <summary>
        /// 可以選擇查詢物件。選項：transaction, order。預設為所有。
        /// </summary>
        [JsonPropertyName("fields")]
        public string Fields { get; set; }
    }
}