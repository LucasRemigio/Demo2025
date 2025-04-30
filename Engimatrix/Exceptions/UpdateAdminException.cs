// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class UpdateAdminException : Exception
    {
#pragma warning disable SA1600

        public UpdateAdminException()
        {
        }

        public UpdateAdminException(string message)
            : base(message)
        {
        }

        public UpdateAdminException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UpdateAdminException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
