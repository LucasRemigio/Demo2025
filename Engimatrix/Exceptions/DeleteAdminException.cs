// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class DeleteAdminException : Exception
    {
#pragma warning disable SA1600

        public DeleteAdminException()
        {
        }

        public DeleteAdminException(string message)
            : base(message)
        {
        }

        public DeleteAdminException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DeleteAdminException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
