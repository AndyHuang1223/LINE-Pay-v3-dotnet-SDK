namespace LinePayApiSdk.DTOs
{
    public class LinePayApiOptions
    {
        /// <summary>
        /// Line提供的ChannelId，可在商家後台查看。
        /// </summary>
        public string ChannelId { get; set; }
        /// <summary>
        /// Line提供的ChannelSecret，可在商家後台查看。
        /// </summary>
        public string ChannelSecret { get; set; }
        /// <summary>
        /// 由呼叫端提供HttpClient，以便可以共用HttpClient。
        /// </summary>
        public HttpClient HttpClient { get; set; }
        /// <summary>
        /// 是否使用LinePay模擬付款環境，當`BaseAddress`有帶值的時候，不論此值為何，都會使用`BaseAddress`
        /// </summary>
        public bool IsSandBox { get; set; }
        /// <summary>
        /// LinePay付款BaseAddress，可由呼叫端自行設定，若未設定，則會依據`IsSendBox`判斷使用的BaseAddress。
        /// </summary>
        public string BaseAddress { get; set; }
    }
}