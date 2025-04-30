// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.ResponseMessages
{
    using System.Collections.Generic;

    /// <summary>
    /// This class corresponds to json structure of response messages.
    /// </summary>
    public class ResponseMessageJSON
    {
#pragma warning disable SA1600

        public class ResponseMessageJSONArray
        {
            public ResponseMessageJSONArray(List<ResponseMessageJSONObject> messages)
            {
                this.Messages = messages;
            }

            public List<ResponseMessageJSONObject> Messages { get; set; }
        }

        public class ResponseMessageJSONObject
        {
            public ResponseMessageJSONObject(int code, string message)
            {
                this.Code = code;
                this.Message = message;
            }

            public int Code { get; set; }

            public string Message { get; set; }
        }
    }

#pragma warning restore SA1600
}
