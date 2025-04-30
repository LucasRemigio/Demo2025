// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class InvalidLoginException : Exception
    {
#pragma warning disable SA1600

        public InvalidLoginException()
        {
        }

        public InvalidLoginException(string message)
            : base(message)
        {
        }

        public InvalidLoginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }

    public class PasswordExpiredException : Exception
    {
        public PasswordExpiredException() : base("Password expired")
        {
        }
    }
}
