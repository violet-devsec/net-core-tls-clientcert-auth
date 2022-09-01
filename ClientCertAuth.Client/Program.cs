using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ClientCertAuth.Client.Helpers;

namespace ClientCertAuth.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Requesting data...");
            await GetData();
        }

        public static async Task<string> GetData()
        {
            try
            {
                HttpHelper httpClient = new HttpHelper(new X509Certificate2(File.ReadAllBytes("user_cert.pfx"), "<cert_pass>"));

                var response = await httpClient.GetAsync("https://clientcertauthapp.azurewebsites.net/weatherforecast");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Response: " + await response.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine("Response: " + response.StatusCode);
                    Console.WriteLine("Reason: " + await response.Content.ReadAsStringAsync());
                }

                Console.WriteLine("Press any key to close...");
                Console.Read();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex.Message);
                return null;
            }
        }
    }
}
