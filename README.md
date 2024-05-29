# LINE Pay API v3 SDK for .NET
## Overview
- This is a .NET SDK for LINE Pay API v3.
- This SDK is based on the official LINE Pay API v3 document.
- LINE Pay API document: [LINE Pay Online APIs](https://pay.line.me/th/developers/apis/onlineApis?locale=zh_TW)
## Features
- This SDK supports all LINE Pay API v3.
	- [x] Request API
	- [x] Confirm API
	- [x] Capture API
	- [x] Void API
	- [x] Refund API
	- [x] Payment Details API
	- [x] Check Payment Status API
	- [x] Check RegKey API
	- [x] Pay Preapproved API
	- [x] Expire RegKey API
## Note
- LINE Pay API URL
	- Production Environment: `https://api-pay.line.me`.
	- Sandbox Environment: `https://sandbox-api-pay.line.me`.
- Apply for a Sandbox merchant account: [LINE Pay create Sandbox](https://pay.line.me/tw/developers/techsupport/sandbox/creation?locale=zh_TW)
- Sandbox API can only simulate a normal transaction processes.
	- `Request API` : Simulate checkout, but no cancel process.
- You can use the Sandbox merchant account to call the Production API for actual payment testing, and the actual payment received by the Sandbox merchant account will be automatically refunded every night.
## Example
```csharp
// 建立 LinePayApi 物件
var linePayApiOptions = new LinePayApiOptions
{
	ChannelId = "YOUR_CHANNEL_Id",
	ChannelSecret = "YOUR_Channel_Secret",
	HttpClient = new HttpClient(),	// 建議透過 IHttpClientFactory 建立 HttpClient 物件
	IsSandBox = true	// 是否為測試環境
};
var linePayApi = new LinePayApi(linePayApiOptions);

// 設定 callbackUriBaseAddress 為你的專案能對外的網址
var callbackUriBaseAddress = "YOUR_CALLBACK_URI_BASE_ADDRESS";
var callbackUri = new Uri(callbackUriBaseAddress);
// 設定 ConfirmUrl 與 CancelUrl
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
var response = await linePayApi.RequestAsync(paymentRequest);

// 將使用者導向 LINE Pay 付款頁面
return Redirect(response.Info.PaymentUrl.Web);

```
## Sample Project
- LinePayApiWebSample (template: ASP.NET Core MVC)
	- set args: 
		1. set `LinePayApiOptions.ChannelId` in `LinePayController`.
		2. set `LinePayApiOptions.ChannelSecret` in `LinePayController`.
		3. set `LinePayApiOptions.BaseAddress` or `LinePayApiOptions.IsSandBox` in `LinePayController`.
		4. set `callbackUriBaseAddress` (you can use [ngrok](https://ngrok.com/) or [devtunnel](https://learn.microsoft.com/zh-tw/azure/developer/dev-tunnels/get-started?tabs=windows) to expose localhost server) in `LinePayController`.
	- run project: `dotnet run --project LinePayApiWebSample`.
## Reference
- [LINE Pay Online APIs](https://pay.line.me/th/developers/apis/onlineApis?locale=zh_TW)
- [LINE Pay create Sandbox](https://pay.line.me/tw/developers/techsupport/sandbox/creation?locale=zh_TW)
- [devtunnel](https://learn.microsoft.com/zh-tw/azure/developer/dev-tunnels/get-started?tabs=windows)