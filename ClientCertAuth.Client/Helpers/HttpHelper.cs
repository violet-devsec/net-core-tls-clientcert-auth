using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClientCertAuth.Client.Helpers
{
    public class HttpHelper
    {
        public HttpClientHandler Handler { get; set; }
        
        public HttpHelper(X509Certificate2 Certificate)
        {
            var certificate = Certificate;
            Handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (req, cert, chain, err) => { return true; }
            };
            if (certificate != null)
                Handler.ClientCertificates.Add(certificate);
        }

        private HttpClient InitializeClient()
        {
            var httpClient = new HttpClient(Handler);
            return httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                var httpClient = InitializeClient();
                var response = await httpClient.GetAsync(url);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Http Excepton: " + ex.Message);
                return null;
            }
        }
    }
}
