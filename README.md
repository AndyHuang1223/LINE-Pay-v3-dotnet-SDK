# LINE Pay API v3 SDK for .NET

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## Overview

- 這是一個針對 LINE Pay API v3 的 .NET SDK
- 此 SDK 基於 LINE Pay API v3 官方文檔實作
- 支援 .NET 8.0 和 C# 的現代語言特性
- 提供完整的 Dependency Injection 支援
- LINE Pay API 文檔: [LINE Pay Online APIs](https://pay.line.me/th/developers/apis/onlineApis?locale=zh_TW)

## 系統需求

- .NET 8.0 或更高版本
- 支援 ASP.NET Core 應用程式

## Features

此 SDK 支援所有 LINE Pay API v3 功能：

- [x] **Request API** - 付款請求
- [x] **Confirm API** - 付款確認
- [x] **Capture API** - 授權付款捕獲
- [x] **Void API** - 付款取消
- [x] **Refund API** - 付款退款
- [x] **Payment Details API** - 付款詳情查詢
- [x] **Check Payment Status API** - 付款狀態檢查
- [x] **Check RegKey API** - 註冊金鑰狀態檢查
- [x] **Pay Preapproved API** - 預先授權付款
- [x] **Expire RegKey API** - 註冊金鑰過期

## 技術特性

- 🔧 **依賴注入支援**: 原生支援 .NET DI 容器
- 🔒 **型別安全**: 強型別 DTO 和 API 回應
- ⚡ **HttpClient 整合**: 支援 IHttpClientFactory 模式
- 🛠️ **可配置性**: 支援多種配置方式（程式碼、appsettings.json）
- 📝 **完整範例**: 包含 ASP.NET Core MVC 範例專案

## Note

- **LINE Pay API URL**
  - Production Environment: `https://api-pay.line.me`
  - Sandbox Environment: `https://sandbox-api-pay.line.me`
- 申請 Sandbox 商家帳號: [LINE Pay create Sandbox](https://pay.line.me/tw/developers/techsupport/sandbox/creation?locale=zh_TW)
- Sandbox API 只能模擬正常的交易流程
  - `Request API`: 模擬結帳流程，但沒有取消流程
- 您可以使用 Sandbox 商家帳號呼叫正式環境 API 進行實際付款測試，Sandbox 商家帳號收到的實際付款會在每晚自動退款

## 安裝方式

### 透過專案參考

如果您有原始碼，可以直接新增專案參考：

```xml
<ProjectReference Include="path/to/LinePayApiSdk/LinePayApiSdk.csproj" />
```

### 透過 NuGet（未來規劃）

```bash
dotnet add package LinePayApiSdk
```

## 使用方式

### 方式 1: 使用 Dependency Injection（推薦）

在 `Program.cs` 中註冊服務：

```csharp
using LinePayApiSdk.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 使用 Action 委託配置
builder.Services.AddLinePay(options =>
{
    options.ChannelId = "YOUR_CHANNEL_ID";
    options.ChannelSecret = "YOUR_CHANNEL_SECRET";
    options.IsSandBox = true; // 設定為 false 使用正式環境
});

// 或使用 appsettings.json 配置
// builder.Services.AddLinePay(builder.Configuration);

var app = builder.Build();
```

在 Controller 中使用：

```csharp
public class PaymentController : Controller
{
    private readonly ILinePayApi _linePayApi;

    public PaymentController(ILinePayApi linePayApi)
    {
        _linePayApi = linePayApi;
    }

    public async Task<IActionResult> CreatePayment()
    {
        var response = await _linePayApi.RequestAsync(paymentRequest);
        return Redirect(response.Info.PaymentUrl.Web);
    }
}
```

### 方式 2: 直接建立實例

```csharp
// 建立 LinePayApi 物件
var linePayApiOptions = new LinePayApiOptions
{
    ChannelId = "YOUR_CHANNEL_Id",
    ChannelSecret = "YOUR_Channel_Secret",
    HttpClient = new HttpClient(), // 建議透過 IHttpClientFactory 建立 HttpClient 物件
    IsSandBox = true // 是否為測試環境
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

## 配置選項

### appsettings.json 配置範例

```json
{
  "LinePay": {
    "ChannelId": "YOUR_CHANNEL_ID",
    "ChannelSecret": "YOUR_CHANNEL_SECRET",
    "IsSandBox": true,
    "BaseAddress": "https://sandbox-api-pay.line.me",
    "TimeoutSeconds": 60
  }
}
```

### 可用的配置選項

- **ChannelId**: LINE Pay 通道 ID（必填）
- **ChannelSecret**: LINE Pay 通道密鑰（必填）
- **IsSandBox**: 是否為沙盒環境（預設：true）
- **BaseAddress**: API 基礎位址（可選，會根據 IsSandBox 自動設定）
- **TimeoutSeconds**: HTTP 請求逾時秒數（預設：60）

## Sample Project

本專案包含完整的範例專案：**LinePayApiWebSample**（基於 ASP.NET Core MVC）

### 執行範例專案

1. 設定必要參數：

   - 在 `Program.cs` 中設定 `LinePayApiOptions.ChannelId`
   - 在 `Program.cs` 中設定 `LinePayApiOptions.ChannelSecret`
   - 在 `Program.cs` 中設定 `LinePayApiOptions.IsSandBox`
   - 在 `LinePayController` 中設定 `callbackUriBaseAddress`（可使用 [ngrok](https://ngrok.com/) 或 [devtunnel](https://learn.microsoft.com/zh-tw/azure/developer/dev-tunnels/get-started?tabs=windows) 將本機服務對外開放）

2. 執行專案：

   ```bash
   dotnet run --project examples/LinePayApiWebSample
   ```

3. 開啟瀏覽器訪問 `https://localhost:5001`

## API 說明

### 支援的 API 方法

| 方法名稱                    | 說明           | 回傳類型                     |
| --------------------------- | -------------- | ---------------------------- |
| `RequestAsync()`            | 建立付款請求   | `PaymentRequestResponse`     |
| `ConfirmAsync()`            | 確認付款       | `ConfirmResponse`            |
| `CaptureAsync()`            | 捕獲授權付款   | `CaptureResponse`            |
| `VoidAsync()`               | 取消付款       | `VoidResponse`               |
| `RefundAsync()`             | 退款           | `RefundResponse`             |
| `PaymentDetailAsync()`      | 查詢付款詳情   | `PaymentDetailResponse`      |
| `CheckPaymentStatusAsync()` | 檢查付款狀態   | `CheckPaymentStatusResponse` |
| `CheckRegKeyAsync()`        | 檢查註冊金鑰   | `CheckRegKeyResponse`        |
| `PayPreapprovedAsync()`     | 預先授權付款   | `PayPreapprovedResponse`     |
| `ExpireRegKeyAsync()`       | 使註冊金鑰過期 | `ExpireRegKeyResponse`       |

## 貢獻

歡迎提交 Issue 和 Pull Request 來改善這個專案。

## License

此專案使用 MIT License。詳情請參閱 [LICENSE](LICENSE) 檔案。

## Reference

- [LINE Pay Online APIs](https://pay.line.me/th/developers/apis/onlineApis?locale=zh_TW)
- [LINE Pay create Sandbox](https://pay.line.me/tw/developers/techsupport/sandbox/creation?locale=zh_TW)
- [Azure Dev Tunnels](https://learn.microsoft.com/zh-tw/azure/developer/dev-tunnels/get-started?tabs=windows)
- [Dependency Injection 使用指南](doc/DI-USAGE.md)
