using System.Text.Json.Serialization;

namespace LinePayApiSdk.DTOs
{
    public abstract class LinePayApiResponseBase
    {
        /// <summary>
        /// 結果代碼。
        /// </summary>
        [JsonPropertyName("returnCode")]
        public string ReturnCode { get; set; }

        /// <summary>
        /// 結果訊息或失敗原因。例如：該商家無法交易，商家認證資訊錯誤。
        /// 錯誤代碼的 “returnMessage” 以英文呈現，如果沒有訊息，則顯示連字號 (-)。
        /// </summary>
        [JsonPropertyName("returnMessage")]
        public string ReturnMessage { get; set; }
    }
    /*
     * LINE Pay錯誤代碼
     * Code	Description
       1101	買家不是LINE Pay的用戶
       1102	買方被停止交易
       1104	此商家不存在
       1105	此商家無法使用 LINE Pay
       1106	標頭(Header)資訊錯誤
       1110	無法使用的信用卡
       1124	金額錯誤 (scale)
       1141	付款帳戶狀態錯誤
       1142	Balance餘額不足
       1145	正在進行付款
       1150	交易記錄不存在
       1152	該transactionId的交易記錄已經存在
       1153	付款request時的金額與申請confirm的金額不一致
       1154	買家設定為自動付款的信用卡暫時無法使用
       1155	交易編號不符合退款資格
       1159	無付款申請資訊
       1163	可退款日期已過無法退款
       1164	超過退款額度
       1165	已經退款而關閉的交易
       1169	用來確認付款的資訊錯誤（請訪問LINE Pay設置付款方式與密碼認證）
       1170	使用者帳戶的餘額有變動
       1172	該訂單編號(orderId)的交易記錄已經存在
       1177	超過允許查詢的交易數目 (100筆)
       1178	商家不支援該貨幣
       1179	無法處理的狀態
       1180	付款時限已過
       1183	付款金額不能小於 0
       1184	付款金額比付款申請時候的金額還大
       1190	regKey 不存在
       1193	regKey 已過期
       1194	此商家無法使用自動付款
       1197	已在處理使用 regKey 進行的付款
       1198	API重覆呼叫，或者授權更新過程中，呼叫了Capture API（請幾分鐘後重試一下）
       1199	內部請求錯誤
       1264	一卡通MONEY相關錯誤
       1280	信用卡付款時候發生了臨時錯誤
       1281	信用卡付款錯誤
       1282	信用卡授權錯誤
       1283	因有異常交易疑慮暫停交易，請洽LINE Pay客服確認
       1284	暫時無法以信用卡付款
       1285	信用卡資訊不完整
       1286	信用卡付款資訊不正確
       1287	信用卡已過期
       1288	信用卡的額度不足
       1289	超過信用卡付款金額上限
       1290	超過一次性付款的額度
       1291	此信用卡已被掛失
       1292	此信用卡已被停卡
       1293	信用卡驗證碼 (CVN) 無效
       1294	此信用卡已被列入黑名單
       1295	信用卡號無效
       1296	無效的金額
       1298	信用卡付款遭拒絕
       2101	參數錯誤
       2102	JSON 資料格式錯誤
       9000	內部錯誤
     */
}