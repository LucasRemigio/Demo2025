// // Copyright (c) 2024 Engibots. All rights reserved.

namespace Engimatrix.ModelObjs
{
    public class SmartSheet
    {
        public class BodyGetAttachByRowId : Body
        {
            public string rowId { get; set; }
        }

        public class BodyGetSS : Body
        {
            public string saveAttachement { get; set; }
        }

        public class ResponseGetSS : Response
        {
            public string data { get; set; }

            public List<Attachment2Download> attachments { get; set; }
        }

        public class ResponseGetAttachByRowId : Response
        {
            public List<Attachment2Download> attachments { get; set; }
        }

        public class BodyUpdateSS : Body
        {
            public string columnToCheck { get; set; }
            public string valueToCheck { get; set; }
            public string columnNameToUpdate { get; set; }
            public string valueToUpdate { get; set; }
        }

        public class BodyUpdateSSWithAttachment : Body
        {
            public string rowId { get; set; }
            public string fileName { get; set; }
            public string fileContent { get; set; }
            public string fileMimeType { get; set; }
        }

        public class Body
        {
            public string token { get; set; }
            public string smarsheetName { get; set; }
        }

        public class Response
        {
            public int statusCode { get; set; }
            public string message { get; set; }
        }

        public class Attachment2Download
        {
            public string url { get; set; }
            public string fileName { get; set; }
        }
    }
}
