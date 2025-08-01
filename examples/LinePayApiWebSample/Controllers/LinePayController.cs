﻿using LinePayApiSdk;
using LinePayApiSdk.DTOs.Confirm;
using LinePayApiSdk.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace LinePayApiWebSample.Controllers
{
    public class LinePayController : Controller
    {
        private readonly ILinePayApi _linePayApi;

        public LinePayController(ILinePayApi linePayApi)
        {
            _linePayApi = linePayApi ?? throw new ArgumentNullException(nameof(linePayApi));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 設定 callbackUriBaseAddress 為你的專案能對外的網址
            var callbackUriBaseAddress = "YOUR_CALLBACK_URI_BASE_ADDRESS";
            var callbackUri = new Uri(callbackUriBaseAddress);
            var confirmUrl = new Uri(callbackUri, "api/LinePay/Confirm").ToString();
            var cancelUrl = new Uri(callbackUri, "api/LinePay/Cancel").ToString();
            
            // 建立 PaymentRequest 物件
            var paymentRequest = new PaymentRequest
            {
                Currency = "TWD",
                OrderId = Guid.NewGuid().ToString(),
                RedirectUrls = new RedirectUrls
                {
                    ConfirmUrl = confirmUrl,
                    CancelUrl = cancelUrl
                },
                Packages = new List<Package>
                {
                    new Package
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Test Package",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                Name = "Test Product1",
                                Quantity = 1,
                                Price = 100
                            },
                            new Product
                            {
                                Name = "Test Product2",
                                Quantity = 2,
                                Price = 20
                            }
                        }
                    }
                }
            };

            // 發送付款請求
            var response = await _linePayApi.RequestAsync(paymentRequest);

            // 將使用者導向 LINE Pay 付款頁面
            return Redirect(response.Info.PaymentUrl.Web);
        }

        [HttpGet]
        [Route("api/LinePay/Confirm")]
        public async Task<IActionResult> Confirm([FromQuery] string transactionId, [FromQuery] string orderId)
        {
            // 建立 ConfirmRequest 物件
            var confirmRequest = new ConfirmRequest
            {
                Amount = decimal.Truncate(140.0m),
                Currency = "TWD"
            };
            
            // 發送確認付款請求
            var confirmResponse = await _linePayApi.ConfirmAsync(transactionId, confirmRequest);
            ViewData["TransactionId"] = transactionId;
            ViewData["OrderId"] = orderId;

            // 確認付款失敗處理
            if (confirmResponse.ReturnCode != "0000")
            {
                ViewData["ReturnCode"] = confirmResponse.ReturnCode;
                ViewData["ReturnMessage"] = confirmResponse.ReturnMessage;
                return View("ConfirmFailure");
            }

            return View("Confirm");
        }

        [HttpGet]
        [Route("api/LinePay/Cancel")]
        public IActionResult Cancel()
        {
            return View("Cancel");
        }
    }
}