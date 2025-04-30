// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class PasswordNotEqualToOriginalException : Exception
    {
#pragma warning disable SA1600

        public PasswordNotEqualToOriginalException()
        {
        }

        public PasswordNotEqualToOriginalException(string message)
            : base(message)
        {
        }

        public PasswordNotEqualToOriginalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PasswordNotEqualToOriginalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
