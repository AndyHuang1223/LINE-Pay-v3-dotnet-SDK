# LINE Pay API v3 SDK for .NET
## Overview
- This is a .NET SDK for LINE Pay API v3.
- This SDK is based on the official LINE Pay API v3 document.
- API document: [LINE Pay Online APIs](https://pay.line.me/th/developers/apis/onlineApis?locale=zh_TW)
- API list:
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
## Sample Code
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