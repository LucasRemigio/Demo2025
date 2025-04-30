// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class InvalidArgsResetPassException : Exception
    {
#pragma warning disable SA1600

        public InvalidArgsResetPassException()
        {
        }

        public InvalidArgsResetPassException(string message)
            : base(message)
        {
        }

        public InvalidArgsResetPassException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidArgsResetPassException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
