// // Copyright (c) 2024 Engibots. All rights reserved.

namespace engimatrix.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class FirmFileNotFoundException : Exception
    {
#pragma warning disable SA1600

        public FirmFileNotFoundException()
        {
        }

        public FirmFileNotFoundException(string message)
            : base(message)
        {
        }

        public FirmFileNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FirmFileNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

#pragma warning restore SA1600
    }
}
