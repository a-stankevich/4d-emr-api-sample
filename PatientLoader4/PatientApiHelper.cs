using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientLoader
{
    class PatientApiHelper
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly string client_id;
        private readonly string client_secret;
        private readonly string subscription_key;

        public PatientApiHelper(string client_id, string client_secret, string subscription_key)
        {
            this.client_id = client_id;
            this.client_secret = client_secret;
            this.subscription_key = subscription_key;
        }

        private async Task<PatientModel[]> SendRequestAsync(string function, DateTime beginDate, DateTime endDate)
        {
            var uri = new UriBuilder("https://apistage4.azure-api.net/Stage4API/" + function)
            {
                Query = $"beginDate={beginDate.ToString("O")}&endDate={endDate.ToString("O")}"
            };
            var request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
            request.Headers.Add("client_id", client_id);
            request.Headers.Add("client_secret", client_secret);
            request.Headers.Add("Subscription-Key", subscription_key);

            var response = await httpClient.SendAsync(request);
            Console.WriteLine($"Response status code: {response.StatusCode}");
            Console.WriteLine($"Response body: {await response.Content.ReadAsStringAsync()}");
            return await response.Content.ReadAsAsync<PatientModel[]>();
        }

        public Task<PatientModel[]> GetNewPatientsAsync(DateTime beginDate, DateTime endDate)
            => SendRequestAsync("patients", beginDate, endDate);

        public Task<PatientModel[]> GetUpdatedPatientsAsync(DateTime beginDate, DateTime endDate)
            => SendRequestAsync("patients_modified", beginDate, endDate);
    }
}
