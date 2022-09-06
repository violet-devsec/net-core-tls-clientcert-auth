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
            if (clientCert.Thumbprint.Trim().ToUpper() != "2E6E459C1A8BC6348B040CD61C809ED159835B0C")
            {
                return false;
            }

            if (DateTime.Compare(DateTime.UtcNow, clientCert.NotBefore) < 0
                        || DateTime.Compare(DateTime.UtcNow, clientCert.NotAfter) > 0)
            {
                return false;
            }

            // 2. Check subject name of certificate
            bool foundSubject = false;
            string[] certSubjectData = clientCert.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certSubjectData)
            {
                if (String.Compare(s.Trim(), "CN=<expected_subject_name>") == 0)
                {
                    foundSubject = true;
                    break;
                }
            }
            if (!foundSubject) return false;


            // Check issuer name of certificate
            bool foundIssuerCN = false, foundIssuerO = false;
            string[] certIssuerData = clientCert.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certIssuerData)
            {
                if (String.Compare(s.Trim(), "CN=<expected_issuer_name>") == 0)
                {
                    foundIssuerCN = true;
                    if (foundIssuerO) break;
                }

                if (String.Compare(s.Trim(), "O=<expected_issuer_name>") == 0)
                {
                    foundIssuerO = true;
                    if (foundIssuerCN) break;
                }
            }

            if (!foundIssuerCN || !foundIssuerO) return false;

            return true;
        }
    }
}
