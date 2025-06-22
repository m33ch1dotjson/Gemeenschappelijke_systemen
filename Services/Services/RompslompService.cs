using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public interface IRompslompService
    {
        Task<int> SendReservationAsInvoiceAsync(Reservation res, CancellationToken ct);
    }

    public class RompslompService : IRompslompService
    {
        private readonly ILogger<RompslompService> _logger;
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public RompslompService(HttpClient http, IConfiguration config, ILogger<RompslompService> logger)
        {
            _http = http;
            _config = config;
            _logger = logger;

            // Set the Rompslomp base URL with your company ID
            _http.BaseAddress = new Uri("https://api.rompslomp.nl/api/v1/companies/1904369788/");
            var token = _config["Rompslomp:ApiToken"];
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("RompslompService initialized with base URL: {BaseUrl}", _http.BaseAddress);
        }

        /// <summary>
        /// Sends a reservation to Rompslomp as a sales invoice.
        /// </summary>
        public async Task<int> SendReservationAsInvoiceAsync(Reservation res, CancellationToken ct)
        {
            var email = res.GetGuest()?.GetEmail();
            _logger.LogInformation("Starting invoice flow for guest: {Email}", email);

            try
            {
                // Step 1: Get or create the contact in Rompslomp
                var contactId = await CreateContactAsync(email, res, ct);
                _logger.LogInformation("Using contact ID: {ContactId}", contactId);

                // Step 2: Build the sales_invoice payload
                var invoicePayload = new
                {
                    sales_invoice = new
                    {
                        contact_id = contactId,
                        date = DateTime.Today.ToString("yyyy-MM-dd"),
                        due_date = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd"),
                        payment_method = "pay_transfer",
                        currency = "eur",
                        currency_exchange_rate = "1.0",
                        sale_type = "",
                        distance_sale = false,
                        invoice_lines = new[]
                        {
                            new
                            {
                                description = $"Stay from {res.GetCheckInDate():dd-MM} to {res.GetCheckOutDate():dd-MM}",
                                price_per_unit = res.GetTotalPrice().ToString("0.00", CultureInfo.InvariantCulture),
                                vat_rate = "0.21",
                                quantity = "1.0"
                            }
                        }
                    }
                };

                // Step 3: Send invoice to Rompslomp
                var json = JsonSerializer.Serialize(invoicePayload);
                _logger.LogDebug("Sending invoice payload: {Payload}", json);

                var response = await _http.PostAsync("sales_invoices", new StringContent(json, System.Text.Encoding.UTF8, "application/json"), ct);
                var rawContent = await response.Content.ReadAsStringAsync(ct);

                _logger.LogInformation("Invoice POST response status: {Status}", response.StatusCode);
                _logger.LogDebug("Invoice response body (raw): {Body}", rawContent);
                _logger.LogWarning("Body before EnsureSuccessStatusCode: {Body}", rawContent);

                response.EnsureSuccessStatusCode();

                // Step 4: Deserialize response and extract invoice ID
                var body = JsonSerializer.Deserialize<RompslompInvoiceResponse>(rawContent);
                _logger.LogInformation("Invoice successfully created with ID: {InvoiceId}", body?.SalesInvoice?.Id);
                return body?.SalesInvoice?.Id ?? throw new Exception("No invoice ID returned.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reservation as invoice to Rompslomp.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Rompslomp contact by email or creates one if not found.
        /// </summary>
        private async Task<int> CreateContactAsync(string email, Reservation res, CancellationToken ct)
        {
            var guest = res.GetGuest();
            var payload = new
            {
                contact = new
                {
                    is_individual = false,
                    is_supplier = false,
                    company_name = $"{guest?.GetFirstName()} {guest?.GetLastName()}",
                    contact_person_name = $"{guest?.GetFirstName()} {guest?.GetLastName()}",
                    contact_person_email_address = email,
                    address = "Unknown",
                    zipcode = "0000AA",
                    city = "Unknown",
                    country_code = "NL"
                    // Je kunt hier kvk_number, vat_number etc. toevoegen als je die hebt
                }
            };

            var json = JsonSerializer.Serialize(payload);
            _logger.LogDebug("Sending new contact payload: {Payload}", json);

            var response = await _http.PostAsync("contacts", new StringContent(json, Encoding.UTF8, "application/json"), ct);
            var content = await response.Content.ReadAsStringAsync(ct);

            _logger.LogInformation("POST /contacts response: {Status}", response.StatusCode);
            _logger.LogDebug("POST /contacts response body: {Body}", content);

            response.EnsureSuccessStatusCode();

            var created = JsonSerializer.Deserialize<ContactWrapper>(content);
            _logger.LogInformation("Created contact with ID: {Id}", created?.Contact?.Id);
            return created?.Contact?.Id ?? throw new Exception("Contact creation failed.");
        }


    }

    // ======================
    // Response Models (DTOs)
    // ======================

    public class ContactListResponse
    {
        [JsonPropertyName("contacts")]
        public List<ContactDto> Contacts { get; set; }
    }

    public class ContactDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class RompslompInvoiceResponse
    {
        [JsonPropertyName("sales_invoice")]
        public InvoiceDto SalesInvoice { get; set; }
    }

    public class InvoiceDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class ContactWrapper
    {
        [JsonPropertyName("contact")]
        public ContactDto Contact { get; set; }
    }

}
