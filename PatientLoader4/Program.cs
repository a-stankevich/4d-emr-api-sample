using Stage4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientLoader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string client_id = "todo";
            const string client_secret = "todo";
            const string subscription_key = "todo";

            await TestQuotes(client_id, client_secret, subscription_key);
            await ListPatients(client_id, client_secret, subscription_key);
        }

        private static async Task ListPatients(string client_id, string client_secret, string subscription_key)
        {
            var apiHelper = new PatientApiHelper(client_id, client_secret, subscription_key);
            var patients = await apiHelper.GetNewPatientsAsync(DateTime.UtcNow.AddDays(-300), DateTime.UtcNow.AddDays(1));
            Console.WriteLine();
            Console.WriteLine($"Received {patients.Length} patients");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.PatientId} {patient.FirstName} {patient.LastName}");
            }
        }

        private static async Task TestQuotes(string client_id, string client_secret, string subscription_key)
        {
            var facilityLocationId = 2;
            var patientId = 3012;
            var providerId = 2;
            var quoteExternalId = "your_uniq_id_3";

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.4d-emr.com")
            };
            httpClient.DefaultRequestHeaders.Add("Subscription-Key", subscription_key);
            var apiClient = new ApiClient(httpClient);

            // Get Durations
            var durations = await apiClient.DurationsAsync(client_id, client_secret);
            PrintLst("Durations", durations.Take(5), x => new { x.Minutes, x.DisplayText });

            // Get Anesthesia Types 
            var anesthesiaTypes = await apiClient.AnesthesiaTypesAsync(client_id, client_secret);
            PrintLst("Anesthesia Types", anesthesiaTypes.Take(5), x => new { x.Code, x.Description });

            // Get Discount Reasons
            var discountReasons = await apiClient.DiscountReasonsAsync(client_id, client_secret);
            PrintLst("Discount Reasons", discountReasons.Take(5), x => new { x.Id, x.Name });

            // Get Anesthesia Groups
            var anesthesiaGroups = await apiClient.AnesthesiaGroupsAsync(facilityLocationId, client_id, client_secret);
            PrintLst("Anesthesia Groups", anesthesiaGroups.Take(5), x => new { x.Id, x.Name });

            // Create Quote
            var createdQuote = await apiClient.Quote_SaveAsync(new UpdatePriceQuoteVM
            {
                ExternalId = quoteExternalId, // required
                ProcedureDescription = "Procedure A and procedure B", // required
                Patient = new IdNameVM { Id = patientId }, // required
                Provider = new IdNameVM { Id = providerId }, // required
                Duration = durations.Single(x => x.Minutes == 30), // required
                AnesthesiaType = anesthesiaTypes.First(),
                AnesthesiaGroup = anesthesiaGroups.First(),
                Procedures = new List<PriceQuoteProcedureVM>
                {
                    new PriceQuoteProcedureVM
                    {
                        ProcedureName = "Procedure A",
                        Amount = 1000,
                        // optional
                        DiscountAmount = 100,
                        DiscountReason = discountReasons.First(x => x.Id == 2) // Cache
                    },
                    new PriceQuoteProcedureVM
                    {
                        ProcedureName = "Procedure B",
                        Amount = 2000,
                        // optional
                        DiscountAmount = 200,
                        DiscountReason = discountReasons.First(x => x.Id == 2) // Cache
                    }
                },
                Supplies = new List<UpdatePriceQuoteSupplyVM>
                {
                    new UpdatePriceQuoteSupplyVM
                    {
                        ItemTitle = "Botox", // required
                        Quantity = 2,
                        UnitCost = 400,
                    },
                    new UpdatePriceQuoteSupplyVM
                    {
                        ItemTitle = "Scalpel", // required
                        Quantity = 5,
                        UnitCost = 10,
                    }
                },
                TotalAmt = 3550
            }, client_id, client_secret);

            // Get Quote by externalId
            var quote = await apiClient.Quote_GetAsync(null/*quoteId*/, quoteExternalId, client_id, client_secret);

            // Update Quote
            var updateQuote = ConvertToUpdateVM(quote);
            updateQuote.Duration = new DurationVM { Minutes = 45 };
            var updatedQuote = await apiClient.Quote_SaveAsync(updateQuote, client_id, client_secret);

            // List Quotes
            var quotes = await apiClient.Quotes_ListAsync(patientId, client_id, client_secret);
            PrintLst("Quotes", quotes, x => new { x.ExternalId, x.PriceQuoteNo, x.Version, x.ProcedureDescription, x.PriceQuoteStatus });

            // Delete Quote
            await apiClient.Quote_DeleteAsync(null/*quoteId*/, quoteExternalId, client_id, client_secret);

            // List Quotes
            quotes = await apiClient.Quotes_ListAsync(patientId, client_id, client_secret);
            PrintLst("Quotes", quotes, x => new { x.ExternalId, x.PriceQuoteNo, x.Version, x.ProcedureDescription, x.PriceQuoteStatus });
        }

        private static UpdatePriceQuoteVM ConvertToUpdateVM(PriceQuoteVM createdQuote)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(createdQuote);
            var updateQuote = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdatePriceQuoteVM>(json);
            return updateQuote;
        }

        static void PrintLst<T>(string title, IEnumerable<T> list, Func<T, object> toString)
        {
            PrintLst(title, list, x => toString(x).ToString());
        }
        static void PrintLst<T>(string title, IEnumerable<T> list, Func<T, string> toString)
        {
            Console.WriteLine($"{title}:");
            foreach (var item in list)
            {
                Console.WriteLine(toString(item));
            }
        }
    }
}
