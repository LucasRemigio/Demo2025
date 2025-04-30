// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised when an error occurs during a call to the Primavera API.
    /// </summary>
    [Serializable]
    public class PrimaveraApiErrorException : Exception
    {
#pragma warning disable SA1600

        public PrimaveraApiErrorException()
        {
        }

        public PrimaveraApiErrorException(string message)
            : base(message)
        {
        }

        public PrimaveraApiErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PrimaveraApiErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
