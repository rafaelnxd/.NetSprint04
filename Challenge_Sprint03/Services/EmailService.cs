using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Challenge_Sprint03.Services
{
    /// <summary>
    /// Dados devolvidos após a tentativa de envio.
    /// </summary>
    public record EmailResult(bool Success, int Status, string Body);

    public class EmailService
    {
        private readonly HttpClient _http;
        private readonly ILogger<EmailService> _log;
        private readonly string _senderName;
        private readonly string _senderEmail;

        public EmailService(
            IConfiguration cfg,
            IHttpClientFactory factory,
            ILogger<EmailService> logger)
        {
            _log = logger;
            _http = factory.CreateClient("Brevo");

            // Cabeçalhos obrigatórios (Brevo v3)
            _http.DefaultRequestHeaders.Add("api-key", cfg["Brevo:ApiKey"]);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _senderName = cfg["Brevo:SenderName"];
            _senderEmail = cfg["Brevo:SenderEmail"];
        }

        /// <summary>
        /// Envia e-mail transacional via Brevo API.
        /// </summary>
        public async Task<EmailResult> EnviarEmailAsync(
            string destinatario,
            string assunto,
            string html)
        {
            var payload = new
            {
                sender = new { name = _senderName, email = _senderEmail },
                to = new[] { new { email = destinatario } },
                subject = assunto,
                htmlContent = html
            };

            string json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using var resp = await _http.PostAsync("smtp/email", content);
                string bodyText = await resp.Content.ReadAsStringAsync();

                _log.LogInformation("Brevo => {Status}: {Body}",
                                    (int)resp.StatusCode, bodyText);

                return new EmailResult(
                    resp.IsSuccessStatusCode,
                    (int)resp.StatusCode,
                    bodyText);
            }
            catch (Exception ex)
            {
                // devolve stack-trace completo p/ depuração
                _log.LogError(ex, "Exceção ao chamar Brevo");
                return new EmailResult(false, 0, ex.ToString());
            }
        }
    }
}
