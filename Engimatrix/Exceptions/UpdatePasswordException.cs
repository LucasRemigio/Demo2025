// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class UpdatePasswordException : Exception
    {
#pragma warning disable SA1600

        public UpdatePasswordException()
        {
        }

        public UpdatePasswordException(string message)
            : base(message)
        {
        }

        public UpdatePasswordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UpdatePasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
