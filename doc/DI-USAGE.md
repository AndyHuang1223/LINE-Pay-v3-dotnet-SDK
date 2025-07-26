# LinePayApiSdk - Dependency Injection 使用指南

本文檔說明了如何在 .NET 應用程式中使用 Dependency Injection 來配置和使用 LinePayApiSdk。

## 安裝

確保你的專案參考了 `LinePayApiSdk` 專案或 NuGet 套件。

## 基本設定

### 方式 1: 使用 Action 委託配置

在 `Program.cs` 中使用 `AddLinePay` 擴展方法來註冊服務：

```csharp
using LinePayApiSdk.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 註冊 LinePay 服務
builder.Services.AddLinePay(options =>
{
    options.ChannelId = "YOUR_CHANNEL_ID";
    options.ChannelSecret = "YOUR_CHANNEL_SECRET";
    options.IsSandBox = true; // 設定為 false 使用正式環境
});

var app = builder.Build();
```

### 方式 2: 使用配置文件

首先在 `appsettings.json` 中添加 LinePay 配置：

```json
{
  "LinePay": {
    "ChannelId": "YOUR_CHANNEL_ID",
    "ChannelSecret": "YOUR_CHANNEL_SECRET",
    "IsSandBox": true
  }
}
```

然後在 `Program.cs` 中註冊服務：

```csharp
using LinePayApiSdk.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 使用配置文件註冊 LinePay 服務
builder.Services.AddLinePay(builder.Configuration);

// 或指定特定的配置節名稱
// builder.Services.AddLinePay(builder.Configuration, "CustomLinePaySection");

var app = builder.Build();
```

## 在 Controller 中使用

註冊服務後，可以在 Controller 中透過建構函式注入 `ILinePayApi`：

```csharp
using LinePayApiSdk;
using LinePayApiSdk.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly ILinePayApi _linePayApi;

    public PaymentController(ILinePayApi linePayApi)
    {
        _linePayApi = linePayApi;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    {
        try
        {
            var response = await _linePayApi.RequestAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmPayment(string transactionId, [FromBody] ConfirmRequest request)
    {
        try
        {
            var response = await _linePayApi.ConfirmAsync(transactionId, request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

## 在其他服務中使用

也可以在其他服務中注入 `ILinePayApi`：

```csharp
public class PaymentService
{
    private readonly ILinePayApi _linePayApi;

    public PaymentService(ILinePayApi linePayApi)
    {
        _linePayApi = linePayApi;
    }

    public async Task<bool> ProcessPaymentAsync(decimal amount, string orderId)
    {
        var paymentRequest = new PaymentRequest
        {
            Amount = amount,
            Currency = "TWD",
            OrderId = orderId,
            // ... 其他設定
        };

        var response = await _linePayApi.RequestAsync(paymentRequest);
        return response.ReturnCode == "0000";
    }
}
```

記得在 `Program.cs` 中也要註冊你的服務：

```csharp
builder.Services.AddScoped<PaymentService>();
```

## 設定選項

### LinePayApiOptions 屬性

| 屬性            | 類型     | 說明                            | 必填 |
| --------------- | -------- | ------------------------------- | ---- |
| `ChannelId`     | `string` | LINE Pay 提供的 Channel ID      | 是   |
| `ChannelSecret` | `string` | LINE Pay 提供的 Channel Secret  | 是   |
| `IsSandBox`     | `bool`   | 是否使用沙盒環境，預設為 `true` | 否   |

### 環境設定

- **沙盒環境**: `https://sandbox-api-pay.line.me`
- **正式環境**: `https://api-pay.line.me`

API 地址會根據 `IsSandBox` 屬性自動選擇：

- `IsSandBox = true`：使用沙盒環境
- `IsSandBox = false`：使用正式環境

## 向後相容性

為了保持向後相容性，舊的建構函式仍然可用，但已標記為過時：

```csharp
[Obsolete("Please use dependency injection with IOptions<LinePayApiOptions> instead.", false)]
public LinePayApi(LinePayApiOptions options, HttpClient httpClient)
```

建議遷移到新的 DI 方式以獲得更好的測試支援和生命週期管理。

## 測試支援

使用 DI 後，可以輕鬆進行單元測試：

```csharp
[Test]
public async Task Should_Process_Payment_Successfully()
{
    // Arrange
    var mockLinePayApi = new Mock<ILinePayApi>();
    mockLinePayApi.Setup(x => x.RequestAsync(It.IsAny<PaymentRequest>()))
              .ReturnsAsync(new PaymentRequestResponse { ReturnCode = "0000" });

    var service = new PaymentService(mockLinePayApi.Object);

    // Act
    var result = await service.ProcessPaymentAsync(100, "ORDER123");

    // Assert
    Assert.IsTrue(result);
}
```

這樣的設計讓你的程式碼更易於測試和維護。
