using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GiftOfTheGivers.Functions
{
    public class GenerateTaxCertificateFunction
    {
        private readonly ILogger _logger;

        public GenerateTaxCertificateFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GenerateTaxCertificateFunction>();
        }

        [Function("GenerateTaxCertificate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GenerateTaxCertificate")]
            HttpRequestData req)
        {
            _logger.LogInformation("GenerateTaxCertificate function triggered.");

            string donorName = "Anonymous Donor";
            decimal amount = 0;

            if (req.Method == "POST")
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    var donation = JsonSerializer.Deserialize<DonationRequest>(body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (donation != null)
                    {
                        donorName = donation.DonorName ?? donorName;
                        amount = donation.Amount;
                    }
                }
            }
            else
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                donorName = query["donorName"] ?? donorName;
                decimal.TryParse(query["amount"], out amount);
            }

            var certificate = new
            {
                CertificateNumber = $"GOTG-TAX-{DateTime.UtcNow:yyyyMMddHHmmss}",
                IssuedTo = donorName,
                DonationAmount = amount,
                DateIssued = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Organisation = "Gift of the Givers",
                Statement = $"This certifies that {donorName} donated R{amount:F2} to Gift of the Givers and is eligible for a Section 18A tax deduction (dummy certificate for POE demonstration purposes)."
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(certificate, new JsonSerializerOptions { WriteIndented = true }));

            return response;
        }
    }

    public class DonationRequest
    {
        public string? DonorName { get; set; }
        public decimal Amount { get; set; }
    }
}