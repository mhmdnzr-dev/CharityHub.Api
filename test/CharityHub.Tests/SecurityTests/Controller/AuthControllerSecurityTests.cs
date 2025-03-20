namespace CharityHub.Tests.SecurityTests.Controller;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

[TestClass]
public class AuthControllerSecurityTests
{
    private static readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("https://localhost:7260") };

    // تست جلوگیری از حملات تزریق SQL
    [TestMethod]
    public async Task SendOtp_ShouldReturnBadRequest_WhenPhoneNumberIsInjectedWithSql()
    {
        var request = new { PhoneNumber = "' OR 1=1 --" }; // نمونه حمله تزریق SQL
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);

        // انتظار داریم که سرور درخواست را رد کند و BadRequest برگرداند
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // تست جلوگیری از حملات XSS
    [TestMethod]
    public async Task SendOtp_ShouldReturnBadRequest_WhenPhoneNumberContainsXSSAttack()
    {
        var request = new { PhoneNumber = "<script>alert('XSS')</script>" }; // حمله XSS
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);

        // انتظار داریم که سرور درخواست را رد کند و BadRequest برگرداند
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // تست جلوگیری از حملات CSRF
    [TestMethod]
    public async Task SendOtp_ShouldReturnForbidden_WhenCsrfTokenIsMissing()
    {
        var request = new { PhoneNumber = "09203216120" };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // ارسال درخواست بدون Token CSRF در هدر
        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);

        // انتظار داریم که سرور درخواست را رد کند و Forbidden برگرداند
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // تست جلوگیری از حملات Brute Force (با ارسال درخواست‌های متعدد)
    [TestMethod]
    public async Task SendOtp_ShouldReturnTooManyRequests_WhenMultipleRequestsSentInShortTime()
    {
        var request = new { PhoneNumber = "09203216120" };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        int successCount = 0;
        int failureCount = 0;

        for (int i = 0; i < 10; i++)
        {
            var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                successCount++;
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                failureCount++;
            }
            else
            {
                Assert.Fail($"Unexpected status code: {response.StatusCode}");
            }
        }

        Assert.IsTrue(successCount <= 5, "More than 5 successful requests were allowed.");
        Assert.IsTrue(failureCount >= 1, "Rate limiting did not trigger.");
    }


    // تست استفاده از HTTPS (پاسخ درخواست‌ها باید از طریق HTTPS باشد)
    [TestMethod]
    public async Task SendOtp_ShouldUseHttps()
    {
        var request = new { PhoneNumber = "09203216120" };
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/v1/Auth/send-otp", content);

        // بررسی اینکه درخواست باید از HTTPS باشد
        Assert.IsTrue(response.RequestMessage.RequestUri.Scheme == Uri.UriSchemeHttps);
    }
}