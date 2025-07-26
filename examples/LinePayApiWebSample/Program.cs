using LinePayApiSdk.Extensions;

namespace LinePayApiWebSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 方式1: 使用 Action 委派配置 LinePay
            builder.Services.AddLinePay(options =>
            {
                options.ChannelId = "YOUR_CHANNEL_ID";
                options.ChannelSecret = "YOUR_CHANNEL_SECRET";
                options.IsSandBox = true;
            });

            // 方式2: 使用設定檔配置 LinePay (註解掉方式1後使用)
            // builder.Services.AddLinePay(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
