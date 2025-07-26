using System.Security.Cryptography;
using System.Text;

namespace LinePayApiSdk.Helpers
{
    public static class SignatureHelper
    {
        /// <summary>
        /// 計算 HMAC-SHA256 簽名並進行 Base64 編碼。
        /// </summary>
        /// <param name="channelSecret">你的 Channel Secret。</param>
        /// <param name="uri">請求的 URI。</param>
        /// <param name="content">請求的主體。</param>
        /// <param name="nonce">隨機數。</param>
        /// <returns>計算出的簽名。</returns>
        public static string CalculateSignature(string channelSecret, string uri, string content, string nonce)
        {
            var message = channelSecret + uri + content + nonce;
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(channelSecret)))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
            
                // 將雜湊結果進行 Base64 編碼
                string signature = Convert.ToBase64String(hashMessage);
                return signature;
            }
        }
    }
}