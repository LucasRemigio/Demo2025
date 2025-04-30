// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This exception is raised during signup when a employee already exists.
    /// </summary>
    [Serializable]
    internal class ClientExistsException : Exception
    {
#pragma warning disable SA1600

        public ClientExistsException()
        {
        }

        public ClientExistsException(string message)
            : base(message)
        {
        }

        public ClientExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ClientExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
