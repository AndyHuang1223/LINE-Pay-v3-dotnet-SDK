using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Confirm
{
    /// <summary>
    /// 表示確認付款回應的主體。
    /// </summary>
    public class ConfirmResponse : LinePayApiResponseBase
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
        /// 請求付款時，回應的商家唯一訂單編號。
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 作為請求付款的結果，回應的交易序號（19個字符）。
        /// </summary>
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// 授權過期時間（ISO 8601），僅限於完成授權（capture=false）的付款，進行回傳。
        /// </summary>
        [JsonPropertyName("authorizationExpireDate")]
        public string AuthorizationExpireDate { get; set; }

        /// <summary>
        /// 用於自動付款的密鑰（15個字符）。
        /// </summary>
        [JsonPropertyName("regKey")]
        public string RegKey { get; set; }

        /// <summary>
        /// 付款資訊列表。
        /// </summary>
        [JsonPropertyName("payInfo")]
        public List<PayInfo> PayInfo { get; set; }

        /// <summary>
        /// 包裹資訊列表。
        /// </summary>
        [JsonPropertyName("packages")]
        public List<PackageInfo> Packages { get; set; }

        /// <summary>
        /// 商家參考資訊。
        /// </summary>
        [JsonPropertyName("merchantReference")]
        public MerchantReference MerchantReference { get; set; }

        /// <summary>
        /// 配送資訊。
        /// </summary>
        [JsonPropertyName("shipping")]
        public ShippingInfo Shipping { get; set; }
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

        /// <summary>
        /// 用於自動付款的信用卡別名。綁定在LINE Pay的信用卡名。
        /// </summary>
        [JsonPropertyName("creditCardNickname")]
        public string CreditCardNickname { get; set; }

        /// <summary>
        /// 用於自動付款的信用卡品牌。VISA, MASTER, AMEX, DINERS, JCB。
        /// </summary>
        [JsonPropertyName("creditCardBrand")]
        public string CreditCardBrand { get; set; }

        /// <summary>
        /// 被遮罩（Masking）的信用卡號（僅限於台灣商家回應，若您需要，可以向商家中心管理者申請獲取）。格式: **** **** **** 1234。
        /// </summary>
        [JsonPropertyName("maskedCreditCardNumber")]
        public string MaskedCreditCardNumber { get; set; }
    }

    /// <summary>
    /// 表示包裹資訊。
    /// </summary>
    public class PackageInfo
    {
        /// <summary>
        /// 包裹的唯一ID。
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 一個包裹中的商品總價。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 手續費：在付款金額中含手續費時回應。
        /// </summary>
        [JsonPropertyName("userFeeAmount")]
        public decimal UserFeeAmount { get; set; }
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

        /// <summary>
        /// 收貨地址。
        /// </summary>
        [JsonPropertyName("address")]
        public ShippingAddress Address { get; set; }
    }

    /// <summary>
    /// 表示收貨地址。
    /// </summary>
    public class ShippingAddress
    {
        /// <summary>
        /// 收貨國家。
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// 收貨地郵政編碼。
        /// </summary>
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// 收貨地區。
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// 收貨省市區。
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 收貨地址。
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// 詳細地址資訊。
        /// </summary>
        [JsonPropertyName("optional")]
        public string Optional { get; set; }

        /// <summary>
        /// 收貨人名。
        /// </summary>
        [JsonPropertyName("recipient")]
        public Recipient Recipient { get; set; }
    }

    /// <summary>
    /// 表示收貨人資訊。
    /// </summary>
    public class Recipient
    {
        /// <summary>
        /// 收貨人名。
        /// </summary>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// 收貨人姓。
        /// </summary>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// 詳細名資訊。
        /// </summary>
        [JsonPropertyName("firstNameOptional")]
        public string FirstNameOptional { get; set; }

        /// <summary>
        /// 詳細姓資訊。
        /// </summary>
        [JsonPropertyName("lastNameOptional")]
        public string LastNameOptional { get; set; }

        /// <summary>
        /// 收貨人電子郵件。
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// 收貨人電話號碼。
        /// </summary>
        [JsonPropertyName("phoneNo")]
        public string PhoneNo { get; set; }
    }
}