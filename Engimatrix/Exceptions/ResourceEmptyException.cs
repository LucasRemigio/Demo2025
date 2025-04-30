// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised when a resource is empty.
    /// </summary>
    [Serializable]
    public class ResourceEmptyException : Exception
    {
#pragma warning disable SA1600

        public ResourceEmptyException()
        {
        }

        public ResourceEmptyException(string message)
            : base(message)
        {
        }

        public ResourceEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ResourceEmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
