using Microsoft.Extensions.Primitives;
using System.Security.Cryptography.X509Certificates;
using System;

namespace ClientCertAuth.WebApi.Helpers
{
    public static class CertificateValidator
    {
        public static bool Verify(StringValues cert)
        {
            byte[] clientCertBytes = Convert.FromBase64String(cert[0]);
            using X509Certificate2 clientCert = new X509Certificate2(clientCertBytes);
            if (clientCert.Thumbprint != "2E6E459C1A8BC6348B040CD61C809ED159835B0C")
            {
                return false;
            }

            if (DateTime.Compare(DateTime.UtcNow, clientCert.NotBefore) < 0
                        || DateTime.Compare(DateTime.UtcNow, clientCert.NotAfter) > 0)
            {
                return false;
            }

            return true;
        }
    }
}
