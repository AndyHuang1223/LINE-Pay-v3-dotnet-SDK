using System.Text;
using LinePayApiSdk.DTOs;
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
using LinePayApiSdk.Extensions;
using LinePayApiSdk.Helpers;
using Microsoft.AspNetCore.Http;

namespace LinePayApiSdk
{
    public class LinePayApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _channelId;
        private readonly string _channelSecret;
        private const string SandboxApiBaseAddress = "https://sandbox-api-pay.line.me";
        private const string ProductionApiBaseAddress = "https://api-pay.line.me";

        public LinePayApi(LinePayApiOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options is required.");

            if (string.IsNullOrEmpty(options.ChannelId))
                throw new ArgumentNullException(nameof(options.ChannelId), "ChannelId is required.");
            if (string.IsNullOrEmpty(options.ChannelSecret))
                throw new ArgumentNullException(nameof(options.ChannelSecret), "ChannelSecret is required.");

            _channelId = options.ChannelId;
            _channelSecret = options.ChannelSecret;
            _httpClient = options.HttpClient ?? throw new ArgumentNullException(nameof(options.HttpClient), "HttpClient is required");
            if (string.IsNullOrEmpty(options.BaseAddress))
            {
                _httpClient.BaseAddress = options.IsSandBox
                    ? new Uri(SandboxApiBaseAddress)
                    : new Uri(ProductionApiBaseAddress);
            }
            else
            {
                _httpClient.BaseAddress = new Uri(options.BaseAddress);
            }
        }


        #region API Authentication

        /*
         * Hmac Signature
           Algorithm : HMAC-SHA256
           Key : Channel Secret （LINE Pay商家中心提供"Channel Id"和"Channel SecretKey"）
           HTTP Method
           GET : Channel Secret + URI + Query String + nonce
           POST : Channel Secret + URI + Request Body + nonce
           HTTP Method : GET

           Signature = Base64(HMAC-SHA256(Your ChannelSecret, (Your ChannelSecret + URI + Query String + nonce))) Query String : 不包含 " 問號（?）" 的Query String（例如： Name1=Value1&Name2=Value2...）

           HTTP Method : POST

           Signature = Base64(HMAC-SHA256(Your ChannelSecret, (Your ChannelSecret + URI + RequestBody + nonce)))
         */

        /// <summary>
        /// 處理Request Headers(包含驗證資訊)
        /// </summary>
        /// <param name="requestContentString"></param>
        /// <param name="requestUriPath"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetRequestHeadersHandler(string requestContentString, string requestUriPath)
        {
            var headers = new Dictionary<string, string>();
            var nonce = Guid.NewGuid().ToString("N");
            var signature =
                SignatureHelper.CalculateSignature(_channelSecret, requestUriPath, requestContentString, nonce);
            headers.Add("X-LINE-ChannelId", _channelId);
            headers.Add("X-LINE-ChannelSecret", _channelSecret);
            headers.Add("X-LINE-Authorization-Nonce", nonce);
            headers.Add("X-LINE-Authorization", signature);
            return headers;
        }

        /// <summary>
        /// 發起請求時如果有帶Body，則需要透過此方法取得HttpContent
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="body"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private HttpContent GetRequestContentFromBodyHandler<T>(string requestUri, T body) where T : class
        {
            var requestBodyJson = body.ToJson();
            var httpContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            var headers = GetRequestHeadersHandler(requestBodyJson, requestUri);
            foreach (var item in headers)
            {
                httpContent.Headers.Add(item.Key, item.Value);
            }

            return httpContent;
        }

        #endregion

        #region API Request Handler

        /// <summary>
        /// 向LinePay發送Get請求
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="parameter"></param>
        /// <typeparam name="TRequestParameter"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        private async Task<TResponse> GetAsync<TRequestParameter, TResponse>(string requestPath,
            TRequestParameter parameter) where TResponse : LinePayApiResponseBase
        {
            var parameters = RequestHelper.GenerateParameters(parameter);
            var queryString = QueryString.Create(parameters);
            //計算Signature時需要移除QueryString的問號
            var tempStr = queryString.ToString().Replace("?", string.Empty);
            var headers = GetRequestHeadersHandler(tempStr, requestPath);

            var requestUri = requestPath + queryString;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            
            foreach (var item in headers)
            {
                httpRequest.Headers.Add(item.Key, item.Value);
            }
            return await _httpClient.SendAsync(httpRequest)
                .ToResult<TResponse>();
        }

        /// <summary>
        /// 向LinePay發送Get請求
        /// </summary>
        /// <param name="requestPath"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        private async Task<TResponse> GetAsync<TResponse>(string requestPath)
            where TResponse : LinePayApiResponseBase
        {
            return await GetAsync<object, TResponse>(requestPath, null);
        }

        /// <summary>
        /// 向LinePay發送Post請求
        /// </summary>
        /// <param name="requestPath"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        private async Task<TResponse> PostAsync<TResponse>(string requestPath) where TResponse : LinePayApiResponseBase
        {
            return await PostAsync<object, TResponse>(requestPath, null);
        }

        /// <summary>
        /// 向LinePay發送Post請求
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="requestBody"></param>
        /// <typeparam name="TRequestBody"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        private async Task<TResponse> PostAsync<TRequestBody, TResponse>(string requestPath,
            TRequestBody requestBody)
            where TRequestBody : class
            where TResponse : LinePayApiResponseBase
        {
            var httpContent = GetRequestContentFromBodyHandler(requestPath, requestBody);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestPath)
            {
                Content = httpContent
            };
            return await _httpClient.SendAsync(httpRequest).ToResult<TResponse>();
        }

        #endregion

        #region Request API 本API向LINE Pay請求付款資訊。由此，可以設定使用者的交易資訊與付款方式。請求成功，將生成LINE Pay交易序號，可以進行付款與退款。

        /*
         * 本API向LINE Pay請求付款資訊。由此，可以設定使用者的交易資訊與付款方式。請求成功，將生成LINE Pay交易序號，可以進行付款與退款。
         * API Spec
           POST /v3/payments/request

           Connection Timeout : 5秒
           Read Timeout : 20秒
         */

        private const string RequestUri = "/v3/payments/request";

        /// <summary>
        /// 本API向LINE Pay請求付款資訊。由此，可以設定使用者的交易資訊與付款方式。請求成功，將生成LINE Pay交易序號，可以進行付款與退款。
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        public async Task<PaymentRequestResponse> RequestAsync(PaymentRequest paymentRequest) =>
            await PostAsync<PaymentRequest, PaymentRequestResponse>(RequestUri, paymentRequest);

        #endregion

        #region Confirm API 在用戶確認付款後，商家可透過confirmUrl或Check Payment Status API，來完成交易。

        /*
         * 在用戶確認付款後，商家可透過confirmUrl或Check Payment Status API，來完成交易。
         * 如果Request API中"options.payment.capture"被設置為false，意味著該交易的授權與請款分開。
         * 在此情況下，付款完成後，狀態仍然會保持”待請款（授權）”。
         * 因此，需呼叫Capture API進行後續處理，才能完成交易的所有流程。
         * API Spec
           POST /v3/payments/{transactionId}/confirm

           Connection Timeout ：5秒
           Read Timeout : 40秒
         */

        private const string ConfirmUri = "/v3/payments/{0}/confirm";

        /// <summary>
        /// 在用戶確認付款後，商家可透過confirmUrl或Check Payment Status API，來完成交易。 如果Request API中"options.payment.capture"被設置為false，意味著該交易的授權與請款分開。 在此情況下，付款完成後，狀態仍然會保持”待請款（授權）”。因此，需呼叫Capture API進行後續處理，才能完成交易的所有流程。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ConfirmResponse> ConfirmAsync(string transactionId, ConfirmRequest request) =>
            await PostAsync<ConfirmRequest, ConfirmResponse>(string.Format(ConfirmUri, transactionId),
                request);

        #endregion

        #region Capture API 呼叫Request API發出付款請求時，把"options.payment.capture"設置為false的話，當Confirm API完成付款後，該交易轉換為“待請款狀態”。

        /*
         * 呼叫Request API發出付款請求時，把"options.payment.capture"設置為false的話，當Confirm API完成付款後，該交易轉換為“待請款狀態”。
         * 在此情況下，需呼叫Capture API進行後續請款處理，才能完成所有付款流程。
         * API Spec
           POST /v3/payments/authorizations/{transactionId}/capture

           Connection Timeout：5秒
           Read Timeout：60秒
         */

        private const string CaptureUri = "/v3/payments/authorizations/{0}/capture";

        /// <summary>
        /// 呼叫Request API發出付款請求時，把"options.payment.capture"設置為false的話，當Confirm API完成付款後，該交易轉換為“待請款狀態”。在此情況下，需呼叫Capture API進行後續請款處理，才能完成所有付款流程。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CaptureResponse> CaptureAsync(string transactionId, CaptureRequest request) =>
            await PostAsync<CaptureRequest, CaptureResponse>(string.Format(CaptureUri, transactionId), request);

        #endregion

        #region Void API 本API針對授權階段的交易資料進行無效處理。

        /*
         * 本API針對授權階段的交易資料進行無效處理。
         * 因此，透過Confirm API完成授權的交易，將會被取消授權 Void API僅對已授權的交易產生影響，如是已請款的交易，需使用Refund API進行退款。
         * API Spec
           POST /v3/payments/authorizations/{transactionId}/void

           Connection Timeout : 5秒
           Read Timeout : 20秒
         */

        private const string VoidUri = "/v3/payments/authorizations/{0}/void";

        /// <summary>
        /// 本API針對授權階段的交易資料進行無效處理。因此，透過Confirm API完成授權的交易，將會被取消授權 Void API僅對已授權的交易產生影響，如是已請款的交易，需使用Refund API進行退款。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<VoidResponse> VoidAsync(string transactionId) =>
            await PostAsync<VoidResponse>(string.Format(VoidUri, transactionId));

        #endregion

        #region Refund API 本 API 用以取消已付款(購買完成)的交易，並可支援部分退款。呼叫時需要帶入該筆付款的 LINE Pay 原始交易序號(transactionId)。

        /*
         * 本 API 用以取消已付款(購買完成)的交易，並可支援部分退款。呼叫時需要帶入該筆付款的 LINE Pay 原始交易序號(transactionId)。
         * API Spec
           POST /v3/payments/{transactionId}/refund

           Connection Timeout : 5秒
           Read Timeout : 20秒
         */

        private const string RefundUri = "/v3/payments/{0}/refund";

        /// <summary>
        /// 本 API 用以取消已付款(購買完成)的交易，並可支援部分退款。呼叫時需要帶入該筆付款的 LINE Pay 原始交易序號(transactionId)。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RefundResponse> RefundAsync(string transactionId, RefundRequest request) =>
            await PostAsync<RefundRequest, RefundResponse>(string.Format(RefundUri, transactionId), request);

        #endregion

        #region Payment Details API 本API查詢LINE Pay中的交易記錄。您可以查詢授權和購買完成狀態的交易。使用"fields"設定，可以按交易或訂單資訊，選擇查出交易記錄。

        /*
         * 本API查詢LINE Pay中的交易記錄。您可以查詢授權和購買完成狀態的交易。使用"fields"設定，可以按交易或訂單資訊，選擇查出交易記錄。
         * API Spec
           GET /v3/payments

           Connection Timeout : 5秒
           Read Timeout : 20秒
         */

        private const string PaymentDetailUri = "/v3/payments";

        /// <summary>
        /// 本API查詢LINE Pay中的交易記錄。您可以查詢授權和購買完成狀態的交易。使用"fields"設定，可以按交易或訂單資訊，選擇查出交易記錄。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaymentDetailResponse> PaymentDetailAsync(PaymentDetailRequest request)
        {
            return await GetAsync<PaymentDetailRequest, PaymentDetailResponse>(PaymentDetailUri,
                request);
        }

        #endregion

        #region Check Payment Status API 本API查詢LINE Pay付款請求的狀態。

        /*
         * 本API查詢LINE Pay付款請求的狀態。
         * 商家應隔一段時間後直接檢查付款狀態，不透過confirmUrl查看用戶是否已經確認付款，最終判斷交易是否完成。
         * API Spec
           GET /v3/payments/requests/{transactionId}/check

           Connection Timeout ：5秒
           Read Timeout : 20秒
         */

        private const string CheckPaymentStatusUri = "/v3/payments/requests/{0}/check";

        /// <summary>
        /// 本API查詢LINE Pay付款請求的狀態。商家應隔一段時間後直接檢查付款狀態，不透過confirmUrl查看用戶是否已經確認付款，最終判斷交易是否完成。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<CheckPaymentStatusResponse> CheckPaymentStatusAsync(string transactionId)
        {
            return await GetAsync<CheckPaymentStatusResponse>(string.Format(CheckPaymentStatusUri, transactionId));
        }

        #endregion

        #region Check RegKey API 本API查詢已建立的RegKey狀態。

        /*
         * 本API查詢已建立的RegKey狀態。
         * API Spec
           GET /v3/payments/preapprovedPay/{regKey}/check

           Connection Timeout : 5秒
           Read Timeout : 20秒

         */

        private const string CheckRegKeyUri = "/v3/payments/preapprovedPay/{0}/check";

        /// <summary>
        /// 本API查詢已建立的RegKey狀態。
        /// </summary>
        /// <param name="regKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CheckRegKeyResponse> CheckRegKeyAsync(string regKey, CheckRegKeyRequest request)
        {
            return await GetAsync<CheckRegKeyRequest, CheckRegKeyResponse>(string.Format(CheckRegKeyUri, regKey),
                request);
        }

        #endregion

        #region Pay Preapproved API 使用本API之前，需要先使用Request API和Confirm API，設定自動付款。

        /*
         * 使用本API之前，您需要先使用Request API和Confirm API，設定自動付款。
         * 通過Confirm API回應的RegKey，不經使用者確認，可以直接進行付款。
         * API Spec
           POST /v3/payments/preapprovedPay/{regKey}/payment

           Connection Timeout ：5秒
           Read Timeout : 40秒

         */

        private const string PayPreapprovedUri = "/v3/payments/preapprovedPay/{0}/payment";

        /// <summary>
        /// 使用本API之前，您需要先使用Request API和Confirm API，設定自動付款。通過Confirm API回應的RegKey，不經使用者確認，可以直接進行付款。
        /// </summary>
        /// <param name="regKey"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PayPreapprovedResponse> PayPreapprovedAsync(string regKey, PayPreapprovedRequest request) =>
            await PostAsync<PayPreapprovedRequest, PayPreapprovedResponse>(string.Format(PayPreapprovedUri, regKey),
                request);

        #endregion

        #region Expire RegKey API 本API對已建立的RegKey進行過期處理。

        /*
         * 本API對已建立的RegKey進行過期處理。
         * API Spec
           POST /v3/payments/preapprovedPay/{regKey}/expire

           Connection Timeout : 5秒
           Read Timeout : 20秒
         */

        private const string ExpireRegKeyUri = "/v3/payments/preapprovedPay/{0}/expire";

        /// <summary>
        /// 本API對已建立的RegKey進行過期處理。
        /// </summary>
        /// <param name="regKey"></param>
        /// <returns></returns>
        public async Task<ExpiredRegKeyResponse> ExpireRegKeyAsync(string regKey) =>
            await PostAsync<ExpiredRegKeyResponse>(string.Format(ExpireRegKeyUri, regKey));

        #endregion
    }
}