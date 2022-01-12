using System;
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
            var apiHelper = new PatientApiHelper(client_id, client_secret, subscription_key);
            var patients = await apiHelper.GetNewPatientsAsync(DateTime.UtcNow.AddDays(-300), DateTime.UtcNow.AddDays(1));
            Console.WriteLine();
            Console.WriteLine($"Received {patients.Length} patients");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.PatientId} {patient.FirstName} {patient.LastName}");
            }
        }
    }
}
