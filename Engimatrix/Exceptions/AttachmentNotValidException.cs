// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception thrown when an attachment is not valid.
    /// </summary>
    [Serializable]
    public class AttachmentNotValidException : Exception
    {
#pragma warning disable SA1600

        public AttachmentNotValidException()
        {
        }

        public AttachmentNotValidException(string message)
            : base(message)
        {
        }

        public AttachmentNotValidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AttachmentNotValidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
