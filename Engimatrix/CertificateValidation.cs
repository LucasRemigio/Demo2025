// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Security.Cryptography.X509Certificates;

namespace engimatrix
{
    public class CertificateValidation
    {
        public bool ValidateCertificate(X509Certificate2 clientCertificate)
        {
            string[] allowedThumbprints = {
                "29A9A5F27BE9FC5B3DC131D8F10165CF8720E574"
            };
            if (allowedThumbprints.Contains(clientCertificate.Thumbprint))
            {
                return true;
            }
            return false;
        }
    }
}
