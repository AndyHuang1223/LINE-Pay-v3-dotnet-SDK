using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.PaymentDetail
{
    public class PaymentDetailResponse : LinePayApiResponseBase
    {
        /// <summary>
        /// 交易資訊列表。
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
        /// 交易序號（19個字元）。
        /// </summary>
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// 交易日期（ISO-8601）。
        /// </summary>
        [JsonPropertyName("transactionDate")]
        public string TransactionDate { get; set; }

        /// <summary>
        /// 交易分類。
        /// </summary>
        [JsonPropertyName("transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// 付款狀態。
        /// </summary>
        [JsonPropertyName("payStatus")]
        public string PayStatus { get; set; }

        /// <summary>
        /// 商品名稱。
        /// </summary>
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商家名稱。
        /// </summary>
        [JsonPropertyName("merchantName")]
        public string MerchantName { get; set; }

        /// <summary>
        /// 貨幣（ISO 4217）。
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// 授權交易到期時間（ISO-8601）。
        /// </summary>
        [JsonPropertyName("authorizationExpireDate")]
        public string AuthorizationExpireDate { get; set; }

        /// <summary>
        /// 付款資訊列表。
        /// </summary>
        [JsonPropertyName("payInfo")]
        public List<PayInfo> PayInfo { get; set; }

        /// <summary>
        /// 商家參考資訊。
        /// </summary>
        [JsonPropertyName("merchantReference")]
        public MerchantReference MerchantReference { get; set; }

        /// <summary>
        /// 退款列表。
        /// </summary>
        [JsonPropertyName("refundList")]
        public List<RefundInfo> RefundList { get; set; }

        /// <summary>
        /// 原始交易序號（19個字元）。
        /// </summary>
        [JsonPropertyName("originalTransactionId")]
        public long OriginalTransactionId { get; set; }
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
        /// 交易金額（建立交易序號時提供的金額）。在檢視原始交易時，最終交易金額的演算法如下：sum(info[].payInfo[].amount) – sum(refundList[].refundAmount)。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 表示商家參考資訊。
    /// </summary>
    public class MerchantReference
    {
        /// <summary>
        /// 商家支援的卡片類型列表。
        /// </summary>
        [JsonPropertyName("affiliateCards")]
        public List<AffiliateCard> AffiliateCards { get; set; }
    }

    /// <summary>
    /// 表示商家支援的卡片類型。
    /// </summary>
    public class AffiliateCard
    {
        /// <summary>
        /// 交易中若用戶符合商店支援的卡片類型。電子發票載具: MOBILE_CARRIER (功能預設不開啟)，商家會員卡: {類別名稱需與LINE Pay洽談確認}。
        /// </summary>
        [JsonPropertyName("cardType")]
        public string CardType { get; set; }

        /// <summary>
        /// 交易中若用戶符合商店支援的卡片類型所對應的內容值。
        /// </summary>
        [JsonPropertyName("cardId")]
        public string CardId { get; set; }
    }

    /// <summary>
    /// 表示退款資訊。
    /// </summary>
    public class RefundInfo
    {
        /// <summary>
        /// 退款序號（19個字元）。
        /// </summary>
        [JsonPropertyName("refundTransactionId")]
        public long RefundTransactionId { get; set; }

        /// <summary>
        /// 交易分類。PAYMENT_REFUND：退款，PARTIAL_REFUND：部分退款。
        /// </summary>
        [JsonPropertyName("transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// 退款金額。
        /// </summary>
        [JsonPropertyName("refundAmount")]
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款日期（ISO-8601）。
        /// </summary>
        [JsonPropertyName("refundTransactionDate")]
        public string RefundTransactionDate { get; set; }
    }
}