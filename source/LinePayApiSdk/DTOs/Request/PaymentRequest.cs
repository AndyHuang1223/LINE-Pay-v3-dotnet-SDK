using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.Request
{
    /// <summary>
    /// 表示付款請求的主體。
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// 付款金額。
        /// = sum(packages[].amount) + sum(packages[].userFee) + options.shipping.feeAmount
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount =>
            Packages?.Sum(p => p.Amount + (p.UserFee ?? 0)) + (Options?.Shipping?.FeeAmount ?? 0) ??
            throw new ArgumentNullException(nameof(Packages), "Packages is required.");

        /// <summary>
        /// 貨幣（ISO 4217）。支援貨幣：USD、JPY、TWD、THB。
        /// 預設為`TWD`。
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "TWD";

        /// <summary>
        /// 商家訂單編號，商家管理的唯一ID。
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 包裹列表。
        /// </summary>
        [JsonPropertyName("packages")]
        public List<Package> Packages { get; set; }

        /// <summary>
        /// 付款操作後跳轉的URL。
        /// </summary>
        [JsonPropertyName("redirectUrls")]
        public RedirectUrls RedirectUrls { get; set; }

        /// <summary>
        /// 付款請求的其他選項。
        /// </summary>
        [JsonPropertyName("options")]
        public Options Options { get; set; }
    }

    /// <summary>
    /// 表示付款請求中的一個包裹。
    /// </summary>
    public class Package
    {
        /// <summary>
        /// 包裹的唯一ID。
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 包裹中的商品總價。
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount =>
            Products?.Sum(p => p.Price * p.Quantity) ??
            throw new ArgumentNullException(nameof(Products), "Products is required.");

        /// <summary>
        /// 手續費，若包含手續費時設定。(非必填)
        /// </summary>
        [JsonPropertyName("userFee")]
        public decimal? UserFee { get; set; }

        /// <summary>
        /// 包裹名稱或商店名稱。(非必填)
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 包裹中的商品列表。
        /// </summary>
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }

    /// <summary>
    /// 表示包裹中的一個商品。
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 商家商品ID。(非必填)
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 商品名稱。
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 商品圖示的URL。(非必填)
        /// </summary>
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 商品數量。
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 各商品付款金額。
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// 各商品原金額。(非必填)
        /// </summary>
        [JsonPropertyName("originalPrice")]
        public decimal? OriginalPrice { get; set; }
    }

    /// <summary>
    /// 表示付款操作後跳轉的URL。
    /// </summary>
    public class RedirectUrls
    {
        /// <summary>
        /// 在Android環境切換應用時所需的資訊，用於防止網路釣魚攻擊（phishing）。(非必填)
        /// </summary>
        [JsonPropertyName("appPackageName")]
        public string AppPackageName { get; set; }

        /// <summary>
        /// 使用者授權付款後，跳轉到該商家URL。
        /// </summary>
        [JsonPropertyName("confirmUrl")]
        public string ConfirmUrl { get; set; }

        /// <summary>
        /// 使用者授權付款後，跳轉的confirmUrl類型。(非必填)
        /// `CLIENT`:使用者的畫面跳轉到商家confirmUrl，完成付款流程
        /// `SERVER`:LINE Pay Server向Merchant Server請求confirmUrl
        /// `NONE`:使用者確認付款請求後，無需顯示付款完成頁的特殊情況下（如線下付款等），不用請求confirmUrl；商家通過Payment Status API，週期性檢查使用者是否確認付款請求
        /// </summary>
        [JsonPropertyName("confirmUrlType")]
        public string ConfirmUrlType { get; set; }

        /// <summary>
        /// 使用者通過LINE付款頁，取消付款後跳轉到該URL。
        /// </summary>
        [JsonPropertyName("cancelUrl")]
        public string CancelUrl { get; set; }
    }

    /// <summary>
    /// 表示付款請求的其他選項。
    /// </summary>
    public class Options
    {
        /// <summary>
        /// 與付款相關的選項。
        /// </summary>
        [JsonPropertyName("payment")]
        public Payment Payment { get; set; }

        /// <summary>
        /// 與顯示相關的選項。
        /// </summary>
        [JsonPropertyName("display")]
        public Display Display { get; set; }

        /// <summary>
        /// 與運送相關的選項。
        /// </summary>
        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }

        /// <summary>
        /// 與家庭服務相關的選項。
        /// </summary>
        [JsonPropertyName("familyService")]
        public FamilyService FamilyService { get; set; }

        /// <summary>
        /// 付款請求的額外選項。
        /// </summary>
        [JsonPropertyName("extra")]
        public Extra Extra { get; set; }
    }

    /// <summary>
    /// 表示與付款相關的選項。
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// 是否自動請款。(非必填)
        /// true(預設)：呼叫Confirm API，統一進行授權/請款處理
        /// false：呼叫Confirm API只能完成授權，需要呼叫Capture API完成請款
        /// </summary>
        [JsonPropertyName("capture")]
        public bool? Capture { get; set; }

        /// <summary>
        /// 付款類型。(非必填)
        /// `NORMAL`: 一般付款
        /// `PREAPPROVED`: 自動付款
        /// </summary>
        [JsonPropertyName("payType")]
        public string PayType { get; set; }
    }

    /// <summary>
    /// 表示與顯示相關的選項。
    /// </summary>
    public class Display
    {
        /// <summary>
        /// 等待付款頁的語言程式碼，預設為英文（en）。(非必填)
        /// 支援語言：en、ja、ko、th、zh_TW、zh_CN。
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// 檢查將用於訪問confirmUrl的瀏覽器。(非必填)
        /// true：如果跟請求付款的瀏覽器不同，引導使用LINE Pay請求付款的瀏覽器。
        /// false：無需檢查瀏覽器，直接訪問confirmUrl。
        /// </summary>
        [JsonPropertyName("checkConfirmUrlBrowser")]
        public bool? CheckConfirmUrlBrowser { get; set; }
    }

    /// <summary>
    /// 表示與運送相關的選項。
    /// </summary>
    public class Shipping
    {
        /// <summary>
        /// 收貨地選項。(非必填)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 運費。(非必填)
        /// </summary>
        [JsonPropertyName("feeAmount")]
        public decimal? FeeAmount { get; set; }

        /// <summary>
        /// 查詢配送方式的URL。(非必填)
        /// </summary>
        [JsonPropertyName("feeInquiryUrl")]
        public string FeeInquiryUrl { get; set; }

        /// <summary>
        /// 運費查詢類型。(非必填)
        /// </summary>
        [JsonPropertyName("feeInquiryType")]
        public string FeeInquiryType { get; set; }

        /// <summary>
        /// 收貨地址的詳細信息。(非必填)
        /// </summary>
        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }

    /// <summary>
    /// 表示收貨地址的詳細信息。
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 收貨國家。(非必填)
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// 收貨地郵政編碼。(非必填)
        /// </summary>
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// 收貨地區。(非必填)
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// 收貨省市區。(非必填)
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// 收貨地址。(非必填)
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// 詳細地址資訊。(非必填)
        /// </summary>
        [JsonPropertyName("optional")]
        public string Optional { get; set; }

        /// <summary>
        /// 收貨人詳細資訊。(非必填)
        /// </summary>
        [JsonPropertyName("recipient")]
        public Recipient Recipient { get; set; }
    }

    /// <summary>
    /// 表示收貨人詳細資訊。
    /// </summary>
    public class Recipient
    {
        /// <summary>
        /// 收貨人名。(非必填)
        /// </summary>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// 收貨人姓。(非必填)
        /// </summary>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// 詳細名資訊。(非必填)
        /// </summary>
        [JsonPropertyName("firstNameOptional")]
        public string FirstNameOptional { get; set; }

        /// <summary>
        /// 詳細姓資訊。(非必填)
        /// </summary>
        [JsonPropertyName("lastNameOptional")]
        public string LastNameOptional { get; set; }

        /// <summary>
        /// 收貨人電子郵件。(非必填)
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// 收貨人電話號碼。(非必填)
        /// </summary>
        [JsonPropertyName("phoneNo")]
        public string PhoneNo { get; set; }
    }

    /// <summary>
    /// 表示與家庭服務相關的選項。
    /// </summary>
    public class FamilyService
    {
        /// <summary>
        /// 新增好友的服務列表。(非必填)
        /// </summary>
        [JsonPropertyName("addFriends")]
        public List<AddFriend> AddFriends { get; set; }
    }

    /// <summary>
    /// 表示要新增的好友。
    /// </summary>
    public class AddFriend
    {
        /// <summary>
        /// 新增好友的服務類型。(非必填)
        /// `lineAt`:支援LINE@新增好友服務
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// 各服務類型的ID列表。(非必填)
        /// </summary>
        [JsonPropertyName("idList")]
        public List<string> IdList { get; set; }
    }

    /// <summary>
    /// 表示額外選項。(非必填)
    /// </summary>
    public class Extra
    {
        /// <summary>
        /// 商店或分店名稱(僅會顯示前100字元)。(非必填)
        /// </summary>
        [JsonPropertyName("branchName")]
        public string BranchName { get; set; }

        /// <summary>
        /// 商店或分店代號，可支援英數字及特殊字元。(非必填)
        /// </summary>
        [JsonPropertyName("branchId")]
        public string BranchId { get; set; }

        /// <summary>
        /// 點數限制資訊。(非必填)
        /// </summary>
        [JsonPropertyName("promotionRestriction")]
        public PromotionRestriction PromotionRestriction { get; set; }
    }

    /// <summary>
    /// 表示點數限制資訊。
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