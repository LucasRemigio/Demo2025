// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a employee already exists.
    /// </summary>
    [Serializable]
    public class AlreadyLoadingException : Exception
    {
#pragma warning disable SA1600

        public AlreadyLoadingException()
        {
        }

        public AlreadyLoadingException(string message)
            : base(message)
        {
        }

        public AlreadyLoadingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AlreadyLoadingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
