namespace LinePayApiSdk.DTOs
{
    public class LinePayApiOptions
    {
        public string ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public HttpClient HttpClient { get; set; }
        /// <summary>
        /// 是否使用LinePay模擬付款環境，當`BaseAddress`有帶值的時候，不論此值為何，都會使用`BaseAddress`
        /// </summary>
        public bool IsSendBox { get; set; }
        /// <summary>
        /// LinePay付款BaseAddress
        /// </summary>
        public string BaseAddress { get; set; }
    }
}