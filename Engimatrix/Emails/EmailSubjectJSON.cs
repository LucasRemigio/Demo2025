// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Emails
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO !!!
    /// </summary>
    public class EmailSubjectJSON
    {
#pragma warning disable SA1600

        public class EmailSubjectJSONArray
        {
            public EmailSubjectJSONArray(List<EmailSubjectJSONObject> messages)
            {
                this.Languages = messages;
            }

            public List<EmailSubjectJSONObject> Languages { get; set; }
        }

        public class EmailSubjectJSONObject
        {
            public EmailSubjectJSONObject(string language, string subject)
            {
                this.Language = language;
                this.Subject = subject;
            }

            public string Language { get; set; }

            public string Subject { get; set; }
        }
    }

#pragma warning restore SA1600
}
