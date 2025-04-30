// // Copyright (c) 2024 Engibots. All rights reserved.

using Smartsheet.Api.Models;

namespace engimatrix.Emails
{
    public class EmailLangKey
    {
        public string EmailRef { get; set; }
        public string Lang { get; set; }

        public EmailLangKey(string emailRef, string lang)
        {
            EmailRef = emailRef;
            Lang = lang;
        }

        public override bool Equals(object obj)
        {
            return obj is EmailLangKey key &&
                   EmailRef == key.EmailRef &&
                   Lang == key.Lang;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EmailRef, Lang);
        }

        public override string ToString()
        {
            return $"Template: {EmailRef}, Language: {Lang}";
        }
    }
}
