using LinePayApiSdk.DTOs.Capture;
using LinePayApiSdk.DTOs.CheckPaymentStatus;
using LinePayApiSdk.DTOs.CheckRegKey;
using LinePayApiSdk.DTOs.Confirm;
using LinePayApiSdk.DTOs.ExpireRegKeyResponse;
using LinePayApiSdk.DTOs.PaymentDetail;
using LinePayApiSdk.DTOs.PayPreapproved;
using LinePayApiSdk.DTOs.Refund;
using LinePayApiSdk.DTOs.Request;
using LinePayApiSdk.DTOs.Void;

namespace LinePayApiSdk
{
    public interface ILinePayApi
    {
        /// <summary>
        /// 本API向LINE Pay請求付款資訊。由此，可以設定使用者的交易資訊與付款方式。請求成功，將生成LINE Pay交易序號，可以進行付款與退款。
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        Task<PaymentRequestResponse> RequestAsync(PaymentRequest paymentRequest);

        /// <summary>
        /// 在用戶確認付款後，商家可透過confirmUrl或Check Payment Status API，來完成交易。 如果Request API中"options.payment.capture"被設置為false，意味著該交易的授權與請款分開。 在此情況下，付款完成後，狀態仍然會保持"待請款（授權）"。因此，需呼叫Capture API進行後續處理，才能完成交易的所有流程。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ConfirmResponse> ConfirmAsync(string transactionId, ConfirmRequest request);

        /// <summary>
        /// 呼叫Request API發出付款請求時，把"options.payment.capture"設置為false的話，當Confirm API完成付款後，該交易轉換為"待請款狀態"。在此情況下，需呼叫Capture API進行後續請款處理，才能完成所有付款流程。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CaptureResponse> CaptureAsync(string transactionId, CaptureRequest request);

        /// <summary>
        /// 本API針對授權階段的交易資料進行無效處理。因此，透過Confirm API完成授權的交易，將會被取消授權 Void API僅對已授權的交易產生影響，如是已請款的交易，需使用Refund API進行退款。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<VoidResponse> VoidAsync(string transactionId);

        /// <summary>
        /// 本 API 用以取消已付款(購買完成)的交易，並可支援部分退款。呼叫時需要帶入該筆付款的 LINE Pay 原始交易序號(transactionId)。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<RefundResponse> RefundAsync(string transactionId, RefundRequest request);

        /// <summary>
        /// 本API查詢LINE Pay中的交易記錄。您可以查詢授權和購買完成狀態的交易。使用"fields"設定，可以按交易或訂單資訊，選擇查出交易記錄。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PaymentDetailResponse> PaymentDetailAsync(PaymentDetailRequest request);

        /// <summary>
        /// 本API查詢LINE Pay付款請求的狀態。商家應隔一段時間後直接檢查付款狀態，不透過confirmUrl查看用戶是否已經確認付款，最終判斷交易是否完成。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<CheckPaymentStatusResponse> CheckPaymentStatusAsync(string transactionId);

        /// <summary>
        /// 本API查詢已建立的RegKey狀態。
        /// </summary>
        /// <param name="regKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CheckRegKeyResponse> CheckRegKeyAsync(string regKey, CheckRegKeyRequest request);

        /// <summary>
        /// 使用本API之前，您需要先使用Request API和Confirm API，設定自動付款。通過Confirm API回應的RegKey，不經使用者確認，可以直接進行付款。
        /// </summary>
        /// <param name="regKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PayPreapprovedResponse> PayPreapprovedAsync(string regKey, PayPreapprovedRequest request);

        /// <summary>
        /// 本API對已建立的RegKey進行過期處理。
        /// </summary>
        /// <param name="regKey"></param>
        /// <returns></returns>
        Task<ExpiredRegKeyResponse> ExpireRegKeyAsync(string regKey);
    }
}
