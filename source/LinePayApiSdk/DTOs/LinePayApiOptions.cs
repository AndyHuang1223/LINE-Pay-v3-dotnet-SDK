namespace LinePayApiSdk.DTOs
{
    public class LinePayApiOptions
    {
        /// <summary>
        /// Line提供的ChannelId，可在商家後台查看。
        /// </summary>
        public string ChannelId { get; set; } = string.Empty;
        
        /// <summary>
        /// Line提供的ChannelSecret，可在商家後台查看。
        /// </summary>
        public string ChannelSecret { get; set; } = string.Empty;
        
        /// <summary>
        /// 是否使用LinePay模擬付款環境
        /// </summary>
        public bool IsSandBox { get; set; } = true;
    }
}