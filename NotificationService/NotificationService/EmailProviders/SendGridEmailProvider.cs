using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationService.Models;

namespace NotificationService.EmailProviders
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SendGridEmailProvider> _logger;
        private readonly string _apiKey;
        private const string SENDGRID_API_URL = "https://api.sendgrid.com/v3/mail/send";

        public SendGridEmailProvider(HttpClient httpClient, ILogger<SendGridEmailProvider> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["EmailProviders:SendGrid:ApiKey"];

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<EmailResponse> SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                var sendGridRequest = new
                {
                    personalizations = new[]
                    {
                        new
                        {
                            to = new[]
                            {
                                new { email = emailRequest.To, name = emailRequest.ToName }
                            },
                            subject = emailRequest.Subject
                        }
                    },
                    from = new { email = emailRequest.From, name = emailRequest.FromName },
                    content = new[]
                    {
                        new { type = "text/plain", value = emailRequest.TextContent },
                        new { type = "text/html", value = emailRequest.HtmlContent }
                    },
                    custom_args = new
                    {
                        tracking_id = emailRequest.TrackingId.ToString()
                    }
                };

                var json = JsonConvert.SerializeObject(sendGridRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Sending email via SendGrid. Tracking ID: {emailRequest.TrackingId}");

                var response = await _httpClient.PostAsync(SENDGRID_API_URL, content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"SendGrid email sent successfully. Tracking ID: {emailRequest.TrackingId}");
                    return new EmailResponse
                    {
                        IsSuccess = true,
                        MessageId = response.Headers.GetValues("X-Message-Id")?.FirstOrDefault(),
                        StatusCode = (int)response.StatusCode,
                        TrackingId = emailRequest.TrackingId
                    };
                }
                else
                {
                    _logger.LogError($"SendGrid API error. Status: {response.StatusCode}, Content: {responseContent}");
                    return new EmailResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        StatusCode = (int)response.StatusCode,
                        TrackingId = emailRequest.TrackingId
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception sending email via SendGrid. Tracking ID: {emailRequest.TrackingId}");
                return new EmailResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 0,
                    TrackingId = emailRequest.TrackingId
                };
            }
        }
    }
}
