namespace CharityHub.Tests.PerformanceTests.Controllers;

using System.Diagnostics;
using System.Text;

using Microsoft.Data.SqlClient;

using Newtonsoft.Json;

[TestClass]
public sealed class AuthControllerPerformanceTests
{
    private static readonly HttpClient _client = new() { BaseAddress = new Uri("https://localhost:7260") };
    private static SqlConnection _sqlConnection;

    // تنظیمات اولیه برای تست‌ها
    [TestInitialize]
    public void TestInit()
    {
        // 1. اتصال به پایگاه داده (در صورت نیاز)
        if (_sqlConnection == null)
        {
            _sqlConnection = new SqlConnection("YourConnectionString");
            _sqlConnection.Open();
        }

        // 2. آماده‌سازی داده‌های تست در پایگاه داده
        // می‌توانید داده‌های مورد نیاز را قبل از هر تست در پایگاه داده قرار دهید
        var command = new SqlCommand("INSERT INTO Users (PhoneNumber, IsVerified) VALUES ('09203216120', 0)", _sqlConnection);
        command.ExecuteNonQuery();

        // 3. اطمینان از اینکه سرویس‌های Mock شده آماده هستند (در صورت استفاده از Moq)
        // _mockService.Setup(s => s.SomeMethod()).ReturnsAsync(someValue);

        // 4. یا می‌توانید داده‌های اولیه را در حافظه بارگذاری کنید، مثلاً در کلاس‌های سرویس‌های mock.
    }

    // پاک‌سازی منابع پس از هر تست
    [TestCleanup]
    public void TestCleanup()
    {
        // 1. پاک‌سازی داده‌های تست از پایگاه داده
        var command = new SqlCommand("DELETE FROM Users WHERE PhoneNumber = '09203216120'", _sqlConnection);
        command.ExecuteNonQuery();

        // 2. بستن اتصال به پایگاه داده
        if (_sqlConnection != null && _sqlConnection.State == System.Data.ConnectionState.Open)
        {
            _sqlConnection.Close();
        }

        // 3. پاک‌سازی mock services (در صورت نیاز)
        // _mockService.Reset();

        // 4. حذف فایل‌های موقتی در صورت نیاز (در صورت استفاده از فایل‌ها برای تست)
        // File.Delete(tempFilePath);
    }


    [TestMethod]
    public async Task SendOtp_PerformanceTest()
    {
        var request = new { PhoneNumber = "09203216120" };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var stopwatch = Stopwatch.StartNew();
        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);
        stopwatch.Stop();

        response.EnsureSuccessStatusCode();
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 500, $"Performance issue: {stopwatch.ElapsedMilliseconds}ms");
    }

    [TestMethod]
    public async Task VerifyOtp_PerformanceTest()
    {
        var request = new { PhoneNumber = "09203216120", OTPCode = "123456" };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var stopwatch = Stopwatch.StartNew();
        var response = await _client.PostAsync("/api/v1/Auth/verify-otp", content);
        stopwatch.Stop();

        response.EnsureSuccessStatusCode();
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 500, $"Performance issue: {stopwatch.ElapsedMilliseconds}ms");
    }


}