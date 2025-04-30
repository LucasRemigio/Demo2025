// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Utils
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using engimatrix.Config;
    using engimatrix.RepositoryRecords;
    using static engimatrix.Utils.Cache;

    public class Cryptography
    {
        private const int JwtTokenExpirationHours = 5;

        public static string Encrypt(string encryptString)
        {
            return Encrypt(encryptString, "");
        }

        public static string Encrypt(string? encryptString, string key)
        {
            if (String.IsNullOrEmpty(encryptString))
            {
                return String.Empty;
            }

            string value_encrypt = encryptString;
            string EncryptionKey = ConfigManager.generalKey + key;
            byte[] clearBytes = Encoding.UTF8.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            string key_encrtypt = encryptString;
            OrderCache.GetCachedOrderEncrypt(key_encrtypt, value_encrypt);
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, "");
        }

        public static string Decrypt(string? cipherText, string key)
        {
            if (String.IsNullOrEmpty(cipherText))
            {
                return String.Empty;
            }

            string cachedOrderValue = OrderCache.GetCachedOrder(cipherText);
            string key_encrtypt = cipherText;

            if (cachedOrderValue != null)
            {
                return cachedOrderValue;
            }

            string EncryptionKey = ConfigManager.generalKey + key;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.UTF8.GetString(ms.ToArray());
                }
            }

            string value_encrypt = cipherText;
            OrderCache.GetCachedOrderEncrypt(key_encrtypt, value_encrypt);
            //Log.Debug($"Decrypted Value = {cipherText}");
            return cipherText;
        }

        public static string GenerateJwtToken(UserDBRecord user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigManager.generalKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(JwtTokenExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static int GetUserJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(ConfigManager.generalKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                return 0;
            }
        }
    }
}
