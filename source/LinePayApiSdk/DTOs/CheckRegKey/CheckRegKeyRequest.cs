using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs.CheckRegKey
{
    public class CheckRegKeyRequest
    {
        /// <summary>
        /// 使用RegKey的信用卡，是否完成預授權。
        /// true : 通過LINE Pay驗證和信用卡預授權，查詢RegKey狀態。請注意，這必須經過LINE Pay管理人員的稽核進行。
        /// false：通過LINE Pay驗證查詢RegKey狀態。
        /// </summary>
        [JsonPropertyName("creditCardAuth")]
        public bool? CreditCardAuth { get; set; }
    }
}