// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised when an error occurs while processing an email
    /// </summary>
    [Serializable]
    public class EmailProcessingException : Exception
    {
#pragma warning disable SA1600

        public EmailProcessingException()
        {
        }

        public EmailProcessingException(string message)
            : base(message)
        {
        }

        public EmailProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmailProcessingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
